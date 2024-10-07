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
    private readonly AdminService _adminService;
    private readonly UserManager<UserAccount> _userManager;
    private readonly MedicalEmployeeService _medicalEmployeeService;

    public AppointmentController(DoctorService doctorService, UserManager<UserAccount> userManager, MedicalEmployeeService medicalEmployeeService, AdminService adminService)
    {
        _doctorService = doctorService;
        _userManager = userManager;
        _medicalEmployeeService = medicalEmployeeService;
        _adminService = adminService;
    }

    // GET: ManageAppointmentsIndex
    //Front page/panel for Medical Employees for managing appointments of doctors from their clinic
    public async Task<IActionResult> ManageAppointmentsIndex()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        long clinicId = 0;
        IList<Doctor> doctors = new List<Doctor>();
        IEnumerable<DoctorDetailsViewModel> doctorsDetails = new List<DoctorDetailsViewModel>();

        if (userId.IsNullOrEmpty()) 
        {
            //Error handling
            return View();
        }

        if (User.IsInRole("App Admin"))
        {
            doctors = await _doctorService.GetAllAsync();
            doctorsDetails = doctors.Select(DoctorDetailsViewModel.CreateViewModelFromEntityModel);
            return View(doctorsDetails);
        }

        if (User.IsInRole("Clinic Admin"))
        {
            clinicId = await _adminService.GetClinicAdminClinicIdByUserAccountIdAsync(long.Parse(userId));
        }
        else if(User.IsInRole("Medical Employee"))
        {
            clinicId = await _medicalEmployeeService.GetMedicalEmployeeClinicIdByUserAccountIdAsync(long.Parse(userId));
        }
       
        if(clinicId == 0)
        {           
            //error handling
            return View();
        }

        doctors = await _doctorService.GetAllDoctorsFromClinic(clinicId);
        doctorsDetails = doctors.Select(DoctorDetailsViewModel.CreateViewModelFromEntityModel);
        return View(doctorsDetails);
    }  
}
