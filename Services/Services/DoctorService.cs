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
    private readonly DoctorClinicsService _doctorClinicsService;

    public DoctorService(MediWebContext context, UserManager<UserAccount> userManager, DoctorClinicsService doctorClinicsService)
        : base(context)
    {
        _userManager = userManager;
        _doctorClinicsService = doctorClinicsService;
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

        return await _set.Include(d => d.UserAccount)
            .Include(d => d.DoctorClinics)
            .ThenInclude(dc => dc.Clinic)
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

        var doctor = doctorDetails.CreateDoctorEntityModel();
        doctor.UserAccountId = user.Id;

        return await AddAsync(doctor);
    }

    public async Task<Doctor> Edit(DoctorDetailsDTO doctorDto)
    {
        var doctor = await GetByIdAsync(doctorDto.Id);
        doctor.UserAccount.FirstName = doctorDto.FirstName;
        doctor.UserAccount.LastName = doctorDto.LastName;
        doctor.Title = doctorDto.Title;
        doctor.UserAccount.Email = doctorDto.Email;

        var identityResult = await _userManager.UpdateAsync(doctor.UserAccount);
        if (!identityResult.Succeeded)
        {
            throw new Exception(identityResult.Errors?.FirstOrDefault()?.ToString());
        }
        return await UpdateAsync(doctor);
    }

    public override async Task<bool> DeleteAsync(long doctorId)
    {
        doctorId.AssertIsNotNull();
        doctorId.AssertIsNotZero();

        var entity = await GetByIdAsync(doctorId)
            ?? throw new Exception("Cannot delete the doctor with Id " + doctorId + "because the doctor with that Id could not be found.");

        await _doctorClinicsService.BulkDeleteDoctorClinicsByDoctorIdAsync(doctorId);
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
