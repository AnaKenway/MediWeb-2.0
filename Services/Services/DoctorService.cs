using Common;
using DataLayer;
using DTOs.UserAccountDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services;

namespace MediWeb.Services;

public class DoctorService : BaseService<Doctor>
{
    private readonly UserManager<UserAccount> _userManager;

    public DoctorService(MediWebContext context, UserManager<UserAccount> userManager)
        : base(context)
    {
        _userManager = userManager;
    }

    public override async Task<IList<Doctor>> GetAllAsync()
    {
        return await _set.Include(d => d.UserAccount)
            .Include(d => d.DoctorClinics)
            .ThenInclude(dc => dc.Clinic)
            .ToListAsync();
    }

    public override async Task<Doctor> GetByIdAsync(long id)
    {
        id.AssertIsNotNull();
        id.AssertIsNotZero();

        return await _set.Include(d => d.DoctorClinics)
            .Include(d => d.UserAccount)
            .SingleOrDefaultAsync(d => d.Id == id) ??
            throw new MediWebClientException(MediWebFeature.CRUD, "Object with given Id doesn't exist.");
    }

    public async Task<Doctor> RegisterDoctorAccount(DoctorDetailsDTO doctorDetails, string password)
    {
        var user = new UserAccount
        {
            UserName = doctorDetails.Email,
            Email = doctorDetails.Email,
            FirstName = doctorDetails.FirstName,
            LastName = doctorDetails.LastName,
            CreatedDate = DateTime.UtcNow
        };

        var identityResult = await _userManager.CreateAsync(user, password);

        if (!identityResult.Succeeded)
        {
            throw new Exception(identityResult.Errors?.FirstOrDefault()?.ToString());
        }

        var doctor = new Doctor
        {
            UserAccountId = user.Id    
        };

        await AddAsync(doctor);

        doctor.DoctorClinics = doctorDetails.DoctorClinics.Select(dc => 
        {
            dc.DoctorId = doctor.Id;
            return dc;
        }).ToList();

        return await UpdateAsync(doctor);

    }

    public async Task<Doctor> Edit(DoctorDetailsDTO doctorDto)
    {
        var doctor = await GetByIdAsync(doctorDto.Id);
        //doctor.ClinicId = doctorDto.ClinicId;
        doctor.UserAccount.FirstName = doctorDto.FirstName;
        doctor.UserAccount.LastName = doctorDto.LastName;
        doctor.UserAccount.Email = doctorDto.Email;

        var identityResult = await _userManager.UpdateAsync(doctor.UserAccount);
        if (!identityResult.Succeeded)
        {
            throw new Exception(identityResult.Errors?.FirstOrDefault()?.ToString());
        }
        return await UpdateAsync(doctor);
    }
}
