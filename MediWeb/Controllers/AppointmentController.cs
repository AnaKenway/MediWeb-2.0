using Common;
using DataLayer;
using MediWeb.Models;
using MediWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MediWeb.Controllers;

[Authorize(Roles = "App Admin,Clinic Admin,Medical Employee")]
public class AppointmentController : Controller
{
    private readonly DoctorService _doctorService;
    private readonly UserManager<UserAccount> _userManager;
    private readonly MedicalEmployeeService _medicalEmployeeService;

    public AppointmentController(DoctorService doctorService, UserManager<UserAccount> userManager, MedicalEmployeeService medicalEmployeeService)
    {
        _doctorService = doctorService;
        _userManager = userManager;
        _medicalEmployeeService = medicalEmployeeService;
    }

    // GET: ManageAppointmentsIndex
    //Front page/panel for Medical Employees for managing appointments of doctors from their clinic
    public async Task<IActionResult> ManageAppointmentsIndex()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        IList<Doctor> doctors = new List<Doctor>();
        IEnumerable<DoctorDetailsViewModel> doctorsDetails = new List<DoctorDetailsViewModel>();

        if (userId.IsNullOrEmpty()) 
        {
            //Error handling
            return View();
        }

        var medicalEmployeeClinicId = await _medicalEmployeeService.GetMedicalEmployeeClinicIdByUserAccountIdAsync(long.Parse(userId));

        if(medicalEmployeeClinicId == 0)
        {
            //Error handling
            //This could also mean that it's an app admin trying to manage content
            //Since the App Admin doesn't belong to any specific clinc
            doctors = await _doctorService.GetAllAsync();
            return View(doctorsDetails);
        }

        doctors = await _doctorService.GetAllDoctorsFromClinic(medicalEmployeeClinicId);
        doctorsDetails = doctors.Select(DoctorDetailsViewModel.CreateViewModelFromEntityModel);
        return View(doctorsDetails);
    }
}
