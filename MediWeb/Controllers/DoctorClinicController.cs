using DataLayer;
using MediWeb.Models;
using MediWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MediWeb.Controllers;

public class DoctorClinicController : Controller
{
    private readonly ClinicService _clinicService;
    private readonly DoctorClinicsService _doctorClinicsService;
    private readonly SpecializationService _specializationService;

    public DoctorClinicController(ClinicService clinicService, SpecializationService specializationService, DoctorClinicsService doctorClinicsService)
    {
        _clinicService = clinicService;
        _specializationService = specializationService;
        _doctorClinicsService = doctorClinicsService;
    }

    // GET: DoctorClinic/Create
    public async Task<IActionResult> Create(long doctorId)
    {
        var clinics = await _clinicService.GetAllAsync();
        var specializations = await _specializationService.GetAllAsync();
        ViewData["DoctorId"] = doctorId;
        ViewData["ClinicId"] = new SelectList(clinics, "Id", "Name");
        ViewData["SpecializationId"] = new SelectList(specializations, "Id", "SpecializationName");
        return View();
    }

    // POST: DoctorClinic/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DoctorClinics doctorClinic)
    {
        if (ModelState.IsValid)
        {

            var doctor = await _doctorClinicsService.AddAsync(doctorClinic);

            return RedirectToAction(nameof(Index));
        }

        var clinics = await _clinicService.GetAllAsync();
        var specializations = await _specializationService.GetAllAsync();
        ViewData["ClinicId"] = new SelectList(clinics, "Id", "Name");
        ViewData["SpecializationId"] = new SelectList(specializations, "Id", "SpecializationName");

        return View(doctorClinic);
    }
 

    // GET: DoctorClinic/Delete/5
    public async Task<IActionResult> Delete(long? id)
    {
        //if (id == null)
        //{
        //    return NotFound();
        //}

        //var doctor = await _doctorService.GetByIdAsync(id.Value);
        //if (doctor == null)
        //{
        //    return NotFound();
        //}

        //var doctorDetails = DoctorDetailsViewModel.CreateViewModelFromEntityModel(doctor);
        //return View(doctorDetails);
    }

    // POST: DoctorClinic/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long id)
    {
        await _doctorClinicsService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
