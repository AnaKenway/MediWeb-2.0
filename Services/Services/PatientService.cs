using Common;
using DataLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services;

namespace MediWeb.Services;

public class PatientService : BaseService<Patient>
{
    private readonly UserManager<UserAccount> _userManager;

    public PatientService(MediWebContext context, UserManager<UserAccount> userManager)
        : base(context)
    {
        _userManager = userManager;
    }

    public async override Task<IList<Patient>> GetAllAsync()
    {
        return await _set.Include(d => d.UserAccount)
            .Include(p => p.Appointments)
            .ToListAsync();
    }

    public async override Task<Patient> GetByIdAsync(long patientId)
    {
        patientId.AssertIsNotNull();
        patientId.AssertIsNotZero();

        return await _set.Include(p => p.UserAccount).Include(p => p.Appointments).SingleOrDefaultAsync(p => p.Id == patientId) ??
            throw new MediWebClientException(MediWebFeature.CRUD, "Object with given Id doesn't exist.");
    }

    public async Task<Patient> EditPatientAsync(Patient patient)
    {
        var existingPatient = await GetByIdAsync(patient.Id) ?? 
            throw new MediWebClientException(MediWebFeature.PatientManagement, "The patient with the given Id doesn't exist");

        existingPatient.UserAccount.FirstName = patient.UserAccount.FirstName;
        existingPatient.UserAccount.LastName = patient.UserAccount.LastName;
        existingPatient.UserAccount.Email = patient.UserAccount.Email;
        existingPatient.Gender = patient.Gender;
        existingPatient.DateOfBirth = patient.DateOfBirth;
        existingPatient.PhoneNumber = patient.PhoneNumber;  
        existingPatient.Jmbg = patient.Jmbg;

        var identityResult = await _userManager.UpdateAsync(existingPatient.UserAccount);
        if (!identityResult.Succeeded)
        {
            throw new Exception(identityResult.Errors?.FirstOrDefault()?.ToString());
        }
        return await UpdateAsync(existingPatient);
    }

    public override async Task<bool> DeleteAsync(long patientId)
    {
        patientId.AssertIsNotNull();
        patientId.AssertIsNotZero();

        var entity = await GetByIdAsync(patientId)
            ?? throw new Exception("Cannot delete the patient with Id " + patientId + "because the patient with that Id could not be found.");

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
