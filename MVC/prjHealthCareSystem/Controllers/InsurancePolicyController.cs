using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjHealthCareSystem.Data;
using prjHealthCareSystem.Models;

namespace prjHealthCareSystem.Controllers
{
    public class InsurancePolicyController : Controller
    {
        //Adding the data class
        private readonly ApplicationDbContext _context;
        public InsurancePolicyController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(DateTime? startTime, DateTime? endDateTime, string status)
        {
            var search = _context.InsurancePolicies
                .Include(i => i.Patient).AsQueryable();
            
            //Search date
            if (startTime.HasValue && endDateTime.HasValue)
            {
                search = search.Where(p =>
                    p.StartDate >= startTime && p.EndDate <= endDateTime);
            }
            //Search Status
            if (!string.IsNullOrEmpty(status))
            {
                search = search.Where(p => p.Status == status);
            }

            return View(await search.ToListAsync());

        }

        //Create method
        public IActionResult Create()
        {
            ViewData["PatientId"] = new SelectList
                (_context.Patients, "Id", "Name");
            return View();
        }

        //create method 
        [HttpPost]
        public async Task<IActionResult> Create(InsurancePolicy policy, IFormFile? upload)
        {
            if(ModelState.IsValid)
            {
                //File uploading 
                if (upload != null && upload.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory()
                        , "wwwroot/uploads");
                    
                    //if the folder does not exits, create it
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    //Create a file name so that there is no overwritting. 
                    var fileName = Guid.NewGuid().ToString() + "_" + upload.FileName;
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    //Open a pipeline to read the file to the hard drive
                    var stream = new FileStream(filePath, FileMode.Create);
                    using (stream)
                    {
                        await upload.CopyToAsync(stream);
                    }
                    //Save the text path to the database so we can find the file later alligator
                    policy.SignedConsentPath = "/uploads/" + fileName;
                }
                //Add the new policy to the database
                _context.Add(policy);
                await _context.SaveChangesAsync();

                return RedirectToAction((nameof(Index)));
            }
            //Repopulate the patient dropdown so it doesnt break when we reload the page
            ViewData["PatientId"] = new SelectList(_context.Patients, 
                "Id", "Name", policy.PatientId);
            //Send back to the form
            return View(policy);
        }

        //Edit 
        public async Task<IActionResult> Edit(int? id)
        {
            var policy = await _context.InsurancePolicies.FindAsync(id);
            ViewData["PatientId"] = new SelectList(_context.Patients, 
                "Id", "Name", policy.PatientId);   
            return View(policy);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, InsurancePolicy policy)
        {
            if(ModelState.IsValid)
            {
                _context.Update(policy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", 
                "Name", policy.PatientId);
            return View(policy);
        }

        //Delete
        public async Task<IActionResult> Delete(int? id)
        {
            var deletePolicy = await _context.InsurancePolicies.
                Include((p => p.Patient))
                .FirstOrDefaultAsync(m => m.Id == id);
            return View(deletePolicy);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var policy = await _context.InsurancePolicies.FindAsync(id);
            if(policy != null)
            {
                _context.InsurancePolicies.Remove(policy);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}