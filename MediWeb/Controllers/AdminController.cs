using Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediWeb.Controllers;

public class AdminController : Controller
{
    private readonly RoleManager<IdentityRole<long>> _roleManager;
    public AdminController(RoleManager<IdentityRole<long>> roleManager)
    {
        _roleManager = roleManager;
    }

    // GET: Admin
    public IActionResult Index()
    {
        return View();
    }

    #region User Roles CRUD

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

}
