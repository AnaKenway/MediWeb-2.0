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

    public MedicalEmployeeService(MediWebContext context, UserManager<UserAccount> userManager)
        : base(context)
    {
        _userManager = userManager;
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
}
