using DataLayer;
using MediWeb.Models;
using MediWeb.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MediWeb.Controllers
{
    public class DoctorController : Controller
    {
        private readonly DoctorService _doctorService;
        private readonly ClinicService _clinicService;

        public DoctorController(DoctorService service, ClinicService clinicService)
        {
            _doctorService = service;
            _clinicService = clinicService;
        }

        // GET: Doctor
        public async Task<IActionResult> Index()
        {
            var doctors = await _doctorService.GetAllAsync();
            var doctorsDetails = doctors.Select(d => DoctorDetailsViewModel.CreateViewModelFromEntityModel(d));
            return View(doctorsDetails);
        }

        // GET: Doctor/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _doctorService.GetByIdAsync(id.Value);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // GET: Doctor/Create
        public async Task<IActionResult> Create()
        {
            var clinics = await _clinicService.GetAllAsync();
            ViewData["ClinicId"] = new SelectList(clinics, "Id", "Name");
            return View();
        }

        // POST: MedicalEmployee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterDoctorViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doctorDto = model.CreateDTOFromViewModel();
                var doctor = await _doctorService.RegisterDoctorAccount(doctorDto, model.Password);

                await _doctorService.AddAsync(doctor);
                return RedirectToAction(nameof(Index));
            }
            var clinics = await _clinicService.GetAllAsync();
            ViewData["ClinicId"] = new SelectList(clinics, "Id", "Name", model.ClinicId);
            return View(model);
        }

        // GET: Doctor/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _doctorService.GetByIdAsync(id.Value);

            if (doctor == null)
            {
                return NotFound();
            }

            var clinics = await _clinicService.GetAllAsync();
            ViewData["ClinicId"] = new SelectList(clinics, "Id", "Name", doctor.Clinics);

            var doctorDetails = DoctorDetailsViewModel.CreateViewModelFromEntityModel(doctor);

            return View(doctorDetails);
        }

        // POST: Doctor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DoctorDetailsViewModel doctorDetails)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var id = doctorDetails.Id;
                    var doctorDto = doctorDetails.CreateDTOFromDetailsViewModel();
                    var result = await _doctorService.Edit(doctorDto);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _doctorService.GetByIdAsync(doctorDetails.Id) == null)
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
            var clinics = await _doctorService.GetAllAsync();
            ViewData["ClinicId"] = new SelectList(clinics, "Id", "Name", doctorDetails.ClinicId);
            return RedirectToAction(nameof(Index));
        }

        // GET: Doctor/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _doctorService.GetByIdAsync(id.Value);
            if (doctor == null)
            {
                return NotFound();
            }

            var doctorDetails = DoctorDetailsViewModel.CreateViewModelFromEntityModel(doctor);
            return View(doctorDetails);
        }

        // POST: Doctor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            await _doctorService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
