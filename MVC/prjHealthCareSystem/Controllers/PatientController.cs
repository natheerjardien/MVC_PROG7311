using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using prjHealthCareSystem.Data;
using prjHealthCareSystem.Models;
using Microsoft.AspNetCore.Routing.Template;

namespace prjHealthCareSystem.Controllers
{
    public class PatientController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View (await _context.Patients.ToListAsync());
            // retunrs the whole list of patients stored in db
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create (Patient patient)
        {
            if (ModelState.IsValid)
            {
                _context.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }

        public async Task<IActionResult> Edit (int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        [HttpPost]
        public async Task<IActionResult> Edit (int id, Patient patient)
        {
            if (id != patient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(patient);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }

            return View(patient);
        }
    }
}