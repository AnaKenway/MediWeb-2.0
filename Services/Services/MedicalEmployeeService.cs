using Common;
using DataLayer;
using DTOs.UserAccountDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services;

namespace MediWeb.Services;

public class MedicalEmployeeService : BaseService<MedicalEmployee>
{
    private readonly UserManager<UserAccount> _userManager;
    private readonly RoleManager<IdentityRole<long>> _roleManager;
    private readonly IdentityRole<long> _medicalStaffRole;
    private readonly long _medicalStaffRoleId = 5;

    public MedicalEmployeeService(MediWebContext context, UserManager<UserAccount> userManager, RoleManager<IdentityRole<long>> roleManager)
        : base(context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _medicalStaffRole = _roleManager.FindByIdAsync(_medicalStaffRoleId.ToString())?.Result ?? new IdentityRole<long>();
    }
    public override async Task<IList<MedicalEmployee>> GetAllAsync()
    {
        return await _set
            .Include(me => me.Clinic)
            .Include(me => me.UserAccount)
            .ToListAsync();
    }

    public override async Task<MedicalEmployee> GetByIdAsync(long id)
    {
        id.AssertIsNotNull();
        id.AssertIsNotZero();

        return await _set
            .Include(me => me.Clinic)
            .Include(me => me.UserAccount)
            .FirstOrDefaultAsync(me => me.Id == id) ??
            throw new MediWebClientException(MediWebFeature.CRUD, "Object with given Id doesn't exist.");
    }

    public async Task<MedicalEmployee> RegisterMedicalEmployeeAccount(MedicalEmployeeDetailsDTO medicalEmployeeDetails, string password)
    {
        var user = new UserAccount
        {
            UserName = medicalEmployeeDetails.Email,
            Email = medicalEmployeeDetails.Email,
            FirstName = medicalEmployeeDetails.FirstName,
            LastName = medicalEmployeeDetails.LastName,
            CreatedDate = DateTime.UtcNow
        };

        var identityResult = await _userManager.CreateAsync(user, password);

        if(!identityResult.Succeeded) 
        {
            throw new Exception(identityResult.Errors?.FirstOrDefault()?.ToString());
        }

        var userRoleResult = await _userManager.AddToRoleAsync(user, _medicalStaffRole.Name);

        if (!userRoleResult.Succeeded)
        {
            throw new Exception(userRoleResult.Errors?.FirstOrDefault()?.ToString());
        }

        var medicalEmployee = new MedicalEmployee
        {
            ClinicId = medicalEmployeeDetails.ClinicId,
            UserAccountId = user.Id
        };

        return await AddAsync(medicalEmployee);
 
    }

    public async Task<MedicalEmployee> Edit(MedicalEmployeeDetailsDTO medicalEmployeeDto)
    {
        var medicalEmployee = await GetByIdAsync(medicalEmployeeDto.Id);
        medicalEmployee.ClinicId = medicalEmployeeDto.ClinicId;
        medicalEmployee.UserAccount.FirstName = medicalEmployeeDto.FirstName;
        medicalEmployee.UserAccount.LastName = medicalEmployeeDto.LastName;
        medicalEmployee.UserAccount.Email = medicalEmployeeDto.Email;

        var identityResult = await _userManager.UpdateAsync(medicalEmployee.UserAccount);
        if(!identityResult.Succeeded) 
        {
            throw new Exception(identityResult.Errors?.FirstOrDefault()?.ToString());
        }
        return await UpdateAsync(medicalEmployee);        
    }

    public override async Task<bool> DeleteAsync(long medicalEmployeeId)
    {
        medicalEmployeeId.AssertIsNotNull();
        medicalEmployeeId.AssertIsNotZero();

        var entity = await GetByIdAsync(medicalEmployeeId)
            ?? throw new Exception("Cannot delete the employee with Id " + medicalEmployeeId + "because the employee with that Id could not be found.");

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
