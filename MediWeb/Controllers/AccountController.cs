using DataLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MediWeb.Models;
using MediWeb.Services;

namespace MediWeb.Controllers;
public class AccountController : Controller
{
    private readonly UserManager<UserAccount> _userManager;
    private readonly SignInManager<UserAccount> _signInManager;
    private readonly PatientService _patientService;

    public AccountController(UserManager<UserAccount> userManager, SignInManager<UserAccount> signInManager, PatientService patientService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _patientService = patientService;
    }

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
}
