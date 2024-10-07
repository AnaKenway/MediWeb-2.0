using Common;
using MediWeb.Models;
using MediWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediWeb.Controllers;

[Authorize(Roles = "App Admin, Clinic Admin")]
public class AdminController : Controller
{
    private readonly RoleManager<IdentityRole<long>> _roleManager;
    private readonly AdminService _adminService;
    public AdminController(RoleManager<IdentityRole<long>> roleManager, AdminService adminService)
    {
        _roleManager = roleManager;
        _adminService = adminService;
    }

    // GET: Admin
    public IActionResult Index()
    {
        return View();
    }

    #region Roles CRUD

    [HttpGet]
    public async Task<IActionResult> ListRoles()
    {
        List<IdentityRole<long>> roles = await _roleManager.Roles.ToListAsync();
        return View(roles);
    }

    [HttpGet]
    public IActionResult CreateRole()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole(IdentityRole<long> roleModel)
    {
        if (!ModelState.IsValid)
        {
            return View(roleModel);
        }

        roleModel.Name.AssertIsNotNullOrEmpty();
        bool roleExists = await _roleManager.RoleExistsAsync(roleModel?.Name);

        if (roleExists)
        {
            ModelState.AddModelError("", "Role Already Exists");
        }
        else
        {
            IdentityRole<long> identityRole = new IdentityRole<long>
            {
                Name = roleModel?.Name
            };

            IdentityResult result = await _roleManager.CreateAsync(identityRole);

            if (result.Succeeded)
            {
                return RedirectToAction("ListRoles", "Admin");
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        return View(roleModel);
    }

    [HttpGet]
    public async Task<IActionResult> EditRole(long roleId)
    {
        roleId.AssertIsNotNull();
        IdentityRole<long> role = await _roleManager.FindByIdAsync(roleId.ToString());

        if (role == null)
        {
            return View();
        }

        return View(role);;
    }

    [HttpPost]
    public async Task<IActionResult> EditRole(IdentityRole<long> roleToEdit)
    {
        if (!ModelState.IsValid)
        {
            return View(roleToEdit);
        }

        var existingRole = await _roleManager.FindByIdAsync(roleToEdit.Id.ToString());

        if (existingRole == null)
        {
            ViewBag.ErrorMessage = $"Role with Id = {roleToEdit.Id} cannot be found";
            return View(roleToEdit);
        }
        else
        {
            existingRole.Name = roleToEdit.Name;

            var result = await _roleManager.UpdateAsync(existingRole);
            if (result.Succeeded)
            {
                return RedirectToAction("ListRoles");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(roleToEdit);
        }
    }

    [HttpPost]
    public async Task<IActionResult> DeleteRole(long roleId)
    {
        roleId.AssertIsNotNull();

        var role = await _roleManager.FindByIdAsync(roleId.ToString());

        if (role == null)
        {
            //TODO: Role not found, handle accordingly
            ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
            return View();
        }

        var result = await _roleManager.DeleteAsync(role);
        if (result.Succeeded)
        {
            return RedirectToAction("ListRoles"); // Redirect to the roles list page
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        return View("ListRoles", await _roleManager.Roles.ToListAsync());
    }

    #endregion

    #region Admin CRUD

    [HttpGet]
    public async Task<IActionResult> ListAdmins()
    {
        var admins = await _adminService.GetAllAsync();
        var adminsDetails = admins.Select(a => AdminDetailsViewModel.CreateViewModelFromEntityModel(a));
        return View(adminsDetails);
    }

    [HttpGet]
    public IActionResult RegisterAdmin()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RegisterAdmin(RegisterAdminViewModel registerModel)
    {
        if (ModelState.IsValid)
        {
            var admin = await _adminService.RegisterAdminAccount(registerModel.ToEntityModel(), registerModel.Password);

            return RedirectToAction(nameof(ListAdmins));
        }
        return View(registerModel);
    }

    [HttpGet]
    public async Task<IActionResult> EditAdmin(long adminId)
    {
        var admin = await _adminService.GetByIdAsync(adminId);
        return View(new AdminDetailsViewModel(admin));
    }

    [HttpPost]
    public async Task<IActionResult> EditAdmin(AdminDetailsViewModel adminDetails)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var result = await _adminService.EditAdminAsync(adminDetails.ToAdminEntityModel());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _adminService.GetByIdAsync(adminDetails.Id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(ListAdmins));
        }
        return RedirectToAction(nameof(ListAdmins));
    }

    [HttpGet]
    public async Task<IActionResult> DeleteAdmin(long adminId)
    {
        adminId.AssertIsNotZero();

        var admin = await _adminService.GetByIdAsync(adminId);
        if (admin == null)
        {
            return NotFound();
        }

        var adminDetails = new AdminDetailsViewModel(admin);
        return View(adminDetails);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteAdminConfirmed(long adminId)
    {
        await _adminService.DeleteAsync(adminId);
        return RedirectToAction(nameof(ListAdmins));
    }

    #endregion

}
