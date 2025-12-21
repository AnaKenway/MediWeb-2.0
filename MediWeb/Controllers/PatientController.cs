using DataLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MediWeb.Models;
using MediWeb.Services;
using Microsoft.EntityFrameworkCore;
using Common;

namespace MediWeb.Controllers;
public class PatientController : Controller
{
    private readonly UserManager<UserAccount> _userManager;
    private readonly SignInManager<UserAccount> _signInManager;
    private readonly PatientService _patientService;
    private readonly RoleManager<IdentityRole<long>> _roleManager;
    private readonly IdentityRole<long> _patientRole;
    private readonly long _patientRoleId = 6;

    public PatientController(UserManager<UserAccount> userManager, SignInManager<UserAccount> signInManager, PatientService patientService, RoleManager<IdentityRole<long>> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _patientService = patientService;
        _roleManager = roleManager;
        _patientRole = _roleManager.FindByIdAsync(_patientRoleId.ToString())?.Result ?? new IdentityRole<long>();
    }

    #region Register and Login

    [HttpGet]
    public IActionResult RegisterPatient()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RegisterPatient(RegisterPatientViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new UserAccount { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, 
                LastName = model.LastName, CreatedDate = DateTime.UtcNow};
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var patient = new Patient { Gender = model.Gender, Jmbg = model.Jmbg, DateOfBirth = model.DateOfBirth,
                    PhoneNumber = model.PhoneNumber, UserAccount = user, UserAccountId = user.Id };

                var userRoleResult = await _userManager.AddToRoleAsync(user, _patientRole.Name);

                if (!userRoleResult.Succeeded)
                {
                    throw new Exception(userRoleResult.Errors?.FirstOrDefault()?.ToString());
                }

                await _patientService.AddAsync(patient);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent:false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    #endregion

    #region Patient (C)RUD

    [HttpGet]
    public async Task<IActionResult> ListPatients()
    {
        var patients = await _patientService.GetAllAsync();
        var patientsDetails = patients.Select(p => new PatientDetailsViewModel(p));
        return View(patientsDetails);
    }

    [HttpGet]
    public async Task<IActionResult> EditPatient(long patientId)
    {
        var patient = await _patientService.GetByIdAsync(patientId);
        return View(new PatientDetailsViewModel(patient));
    }
    
    [HttpPost]
    public async Task<IActionResult> EditPatient(PatientDetailsViewModel patientDetails)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var result = await _patientService.EditPatientAsync(patientDetails.ToPatientEntityModel());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _patientService.GetByIdAsync(patientDetails.Id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(ListPatients));
        }
        return RedirectToAction(nameof(ListPatients));
    }

    // GET: Patient/DeletePatient/5
    public async Task<IActionResult> DeletePatient(long patientId)
    {
        patientId.AssertIsNotZero();

        var patient = await _patientService.GetByIdAsync(patientId);
        if (patient == null)
        {
            return NotFound();
        }

        var patientDetails = new PatientDetailsViewModel(patient);
        return View(patientDetails);
    }

    // POST: Patient/DeletePatientConfirmed/5
    [HttpPost, ActionName("DeletePatientConfirmed")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePatientConfirmed(long patientId)
    {
        await _patientService.DeleteAsync(patientId);
        return RedirectToAction(nameof(ListPatients));
    }

    #endregion
}
