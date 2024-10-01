using Common;
using DataLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services;

namespace MediWeb.Services;

public class AdminService : BaseService<Admin>
{ 
    private readonly UserManager<UserAccount> _userManager;
    private readonly RoleManager<IdentityRole<long>> _roleManager;
    private readonly IdentityRole<long> _appAdminRole;
    private readonly IdentityRole<long> _clinicAdminRole;
    private const long _appAdminRoleId = 1;
    private const long _clinicAdminRoleId = 2;

    public AdminService(MediWebContext context, UserManager<UserAccount> userManager, RoleManager<IdentityRole<long>> roleManager)
        : base(context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _appAdminRole = _roleManager.FindByIdAsync(_appAdminRoleId.ToString())?.Result ?? new IdentityRole<long>();
        _clinicAdminRole = _roleManager.FindByIdAsync(_clinicAdminRoleId.ToString())?.Result ?? new IdentityRole<long>();
    }


    public async Task<Admin> ChangeAdminTypeAsync(long adminId, AdminType newAdminType)
    {
        var admin = await _set.FindAsync(adminId) ??
            throw new MediWebClientException(MediWebFeature.Administration, "Cannot change type of Admin because the Admin with Id " + adminId + " doesn't exist!");
        admin.AdminType = newAdminType;
        return await UpdateAsync(admin);
    }

    public async override Task<IList<Admin>> GetAllAsync()
    {
        return await _set.Include(a => a.UserAccount)
            .ToListAsync();
    }

    public async override Task<Admin> GetByIdAsync(long adminId)
    {
        adminId.AssertIsNotNull();
        adminId.AssertIsNotZero();

        return await _set.Include(a => a.UserAccount).SingleOrDefaultAsync(a => a.Id == adminId) ??
            throw new MediWebClientException(MediWebFeature.CRUD, "Admin with given Id doesn't exist.");
    }

    public async Task<Admin> RegisterAdminAccount(Admin admin, string password)
    {
        admin.UserAccount.CreatedDate = DateTime.UtcNow;

        var identityResult = await _userManager.CreateAsync(admin.UserAccount, password);

        if (!identityResult.Succeeded)
        {
            throw new Exception(identityResult.Errors?.FirstOrDefault()?.ToString());
        }

        var userRoleResult = admin.AdminType is AdminType.AppAdmin 
            ? await _userManager.AddToRoleAsync(admin.UserAccount, _appAdminRole.Name)
            : await _userManager.AddToRoleAsync(admin.UserAccount, _clinicAdminRole.Name);

        if (!userRoleResult.Succeeded)
        {
            throw new Exception(userRoleResult.Errors?.FirstOrDefault()?.ToString());
        }

        admin.UserAccountId = admin.UserAccount.Id;

        return await AddAsync(admin);
    }

    public async Task<Admin> EditAdminAsync(Admin admin)
    {
        var existingAdmin = await GetByIdAsync(admin.Id) ??
            throw new MediWebClientException(MediWebFeature.AccountManagement, "The admin with the given Id doesn't exist");

        existingAdmin.UserAccount.FirstName = admin.UserAccount.FirstName;
        existingAdmin.UserAccount.LastName = admin.UserAccount.LastName;
        existingAdmin.UserAccount.Email = admin.UserAccount.Email;
        existingAdmin.AdminType = admin.AdminType;

        var identityResult = await _userManager.UpdateAsync(existingAdmin.UserAccount);
        if (!identityResult.Succeeded)
        {
            throw new Exception(identityResult.Errors?.FirstOrDefault()?.ToString());
        }
        return await UpdateAsync(existingAdmin);
    }

    public override async Task<bool> DeleteAsync(long adminId)
    {
        adminId.AssertIsNotNull();
        adminId.AssertIsNotZero();

        var entity = await GetByIdAsync(adminId)
            ?? throw new Exception("Cannot delete the admin with Id " + adminId + "because the admin with that Id could not be found.");

        _set.Remove(entity);
        await _userManager.DeleteAsync(entity.UserAccount);
        var result = await _context.SaveChangesAsync();

        if (result > 0)
        {
            return true;
        }

        return false;
    }   
}
