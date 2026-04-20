using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjHealthCareSystem.Data;
using prjHealthCareSystem.Models;

namespace prjHealthCareSystem.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var appointment = _context.Appointment
                .Include(m => m.Policy)
                .ThenInclude(p => p.Patient);

            return View(await appointment.ToListAsync());
        }

        //Create 
        public IActionResult Create()
        {
            ViewData["InsurancePolicyId"] = new SelectList(_context.InsurancePolicies,
                "Id", "PolicyName");
            return View();
        }

        //Create HttpPost
        [HttpPost]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            var policy = await _context.InsurancePolicies.
                FindAsync((appointment.InsurancePolicyId));

            if (policy == null || policy.Status != "Active")
            {
                var errorMessage = "Insurance Policy is not Active";
                ModelState.AddModelError("InsurancePolicyId", errorMessage);
            }

            if (ModelState.IsValid)
            {
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
             ViewData["InsurancePolicyId"] = new SelectList(_context.InsurancePolicies, 
                 "Id", "PolicyName", appointment.InsurancePolicyId);
            return View(appointment);
        }

        //Edit 
        public async Task<IActionResult> Edit(int? id)
        {
            var appointment = await _context.Appointment.FindAsync(id);
            ViewData["InsurancePolicyId"] = new SelectList(_context.InsurancePolicies,
                "Id", "PolicyName", 
                appointment.InsurancePolicyId);
            return View(appointment);
            
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                _context.Update(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["InsurancePolicyId"] = new SelectList(_context.InsurancePolicies, 
                "Id", "PolicyName", 
                appointment.InsurancePolicyId);
            return View(appointment);
        }

        //Delete 
        public async Task<IActionResult> Delete (int? id)
        {
            var appointment = _context.Appointment
                .Include(m => m.Policy)
                .FirstOrDefaultAsync(m => m.Id == id);
            return View(await appointment);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointment.Remove(appointment);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ///////LASSSTTTTT ONNEEEEEEEE
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var appointment =  await _context.Appointment
                .Include(m => m.Policy)
                .ThenInclude(p => p.Patient)
                .FirstOrDefaultAsync(m => m.Id == id); 
            
            if (appointment == null)
            {
                return NotFound();
            }
            return View(appointment);
        }
    }
}