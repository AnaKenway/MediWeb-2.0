using Common;
using DataLayer;
using MediWeb.Models;
using MediWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
        if (!ModelState.IsValid)
        {
            var clinics = await _clinicService.GetAllAsync();
            var specializations = await _specializationService.GetAllAsync();
            ViewData["ClinicId"] = new SelectList(clinics, "Id", "Name");
            ViewData["SpecializationId"] = new SelectList(specializations, "Id", "SpecializationName");

            return View(doctorClinic);
        }  

        await _doctorClinicsService.AddAsync(doctorClinic);
        return RedirectToAction("Index", "Doctor");
    }
 

    // GET: DoctorClinic/Delete/5
    public IActionResult Delete(long doctorId)
    {
        doctorId.AssertIsNotZero();

        var doctorClinics = _doctorClinicsService.GetAllDoctorClinicsByDoctorId(doctorId);
        if (doctorClinics.IsNullOrEmpty())
        {
            return NotFound();
        }
        return View(doctorClinics);
    }

    // POST: DoctorClinic/DeleteConfirmed?doctorId=3&clinicId=3&specializationId=9
    [HttpPost, ActionName("DeleteConfirmed")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long doctorId, long clinicId, long specializationId)
    {
        if (await _doctorClinicsService.DeleteDoctorClinicByIdsAsync(doctorId, clinicId, specializationId))
        {
            //TODO:Add a client message that says the deletion was successful
            return RedirectToAction(nameof(Delete), "DoctorClinic", new { doctorId });
        }
        else
        {
            return RedirectToAction(nameof(Delete), doctorId);
        }
    }
}
