using DataLayer;
using MediWeb.Models;
using MediWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MediWeb.Controllers;

public class DoctorController : Controller
{
    private readonly DoctorService _doctorService;
    private readonly ClinicService _clinicService;
    private readonly SpecializationService _specializationService;

    public DoctorController(DoctorService service, ClinicService clinicService, SpecializationService specializationService)
    {
        _doctorService = service;
        _clinicService = clinicService;
        _specializationService = specializationService;
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
        var specializations = await _specializationService.GetAllAsync();
        ViewData["ClinicId"] = new SelectList(clinics, "Id", "Name");
        ViewData["SpecializationId"] = new SelectList(specializations, "Id", "SpecializationName");
        return View();
    }

    // POST: Doctor/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RegisterDoctorViewModel model)
    {
        if (ModelState.IsValid)
        {
            var doctorDto = model.CreateDTOFromRegisterViewModel();

            var doctor = await _doctorService.RegisterDoctorAccount(doctorDto, model.Password);

            return RedirectToAction(nameof(Index));
        }

        var clinics = await _clinicService.GetAllAsync();
        var specializations = await _specializationService.GetAllAsync();
        ViewData["ClinicId"] = new SelectList(clinics, "Id", "Name");
        ViewData["SpecializationId"] = new SelectList(specializations, "Id", "SpecializationName");

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
        var specializations = await _specializationService.GetAllAsync();
        var selectListClinics = new SelectList(clinics, "Id", "Name");
        ViewData["ClinicId"] = selectListClinics;

        var selectListSpecializations = new SelectList(specializations, "Id", "SpecializationName");
        ViewData["SpecializationId"] = selectListSpecializations;

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

        var clinics = await _clinicService.GetAllAsync();
        var specializations = await _specializationService.GetAllAsync();
        ViewData["ClinicId"] = new SelectList(clinics, "Id", "Name");
        ViewData["SpecializationId"] = new SelectList(specializations, "Id", "SpecializationName");

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
