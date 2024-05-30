using Microsoft.AspNetCore.Mvc;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using MediWeb.Services;
using Common;

namespace MediWeb.Controllers
{
    //IDEA: Move all the null-checking logic from controller, to the service level
    public class ClinicController : Controller
    {
        private readonly ClinicService _clinicService;

        public ClinicController(ClinicService clinicService)
        {
            _clinicService = clinicService;
        }

        // GET: Clinic
        public async Task<IActionResult> Index()
        {
            var clinics = await _clinicService.GetAllAsync();
            return View(clinics);
        }

        // GET: Clinic/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clinic/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Clinic clinic)
        {
            if (ModelState.IsValid)
            {
                await _clinicService.AddAsync(clinic);
                return RedirectToAction(nameof(Index));
            }
            return View(clinic);
        }

        // GET: Clinic/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clinic = await _clinicService.GetByIdAsync((long)id);

            if (clinic == null)
            {
                return NotFound();
            }
            return View(clinic);
        }

        // POST: Clinic/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Clinic clinic)
        {
            if (!ModelState.IsValid)
                return View(clinic);
            try
            {
                await _clinicService.UpdateAsync(clinic);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ClinicExists(clinic.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));                    
        }

        // GET: Clinic/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            id.AssertIsNotNullOrZero();

            var clinic = await _clinicService.GetByIdAsync((long)id);
            if (clinic == null)
            {
                return NotFound();
            }

            return View(clinic);
        }

        // POST: Clinic/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var clinic = await _clinicService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ClinicExists(long id)
        {
            var exists = await _clinicService.GetByIdAsync(id) != null;
            return exists;
        }
    }
}
