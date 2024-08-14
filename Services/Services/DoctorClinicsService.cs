using Common;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using Services;

namespace MediWeb.Services;

public class DoctorClinicsService : BaseService<DoctorClinics>
{
    public DoctorClinicsService(MediWebContext context)
        : base(context)
    {
    }

    public async Task<DoctorClinics> GetDoctorClinicByCompositeKey(long doctorId, long clinicId, long specializationId)
    {
        return await _set.SingleOrDefaultAsync(dc => dc.DoctorId == doctorId && dc.ClinicId == clinicId && dc.SpecializationId == specializationId)
            ?? throw new MediWebClientException(MediWebFeature.DoctorManagement, "The Docotor with Id " + doctorId + 
            ", does not work in Clinic with Id " + clinicId + 
            ", under the Specialization with Id " + specializationId + ".");
    }

    /// <summary>
    /// Returns all doctors that work at the Clinic with the specific Id.
    /// </summary>
    /// <param name="clinicId"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<IEnumerable<Doctor>> GetAllDoctorsFromClinicByIdAsync(long clinicId)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns all Clinics at which the Doctor with the desired Id works at.
    /// A Doctor can work at multiple Clinics.
    /// </summary>
    /// <param name="doctorId"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Clinic>> GetAllClinicsWithDoctorByIdAsync(long doctorId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<DoctorClinics> GetAllDoctorClinicsByDoctorId(long doctorId)
    {
        doctorId.AssertIsNotNull();
        doctorId.AssertIsNotZero();

        return  _set.Where(dc => dc.DoctorId == doctorId);
    }

    public async Task BulkDeleteDoctorClinicsByDoctorIdAsync(long doctorId)
    {
        var doctorClinics = GetAllDoctorClinicsByDoctorId(doctorId);
        await BulkDeleteAsync(doctorClinics);
    }


    public async Task<bool> DeleteDoctorClinicAsync(long doctorId, long clinicId, long specializationId)
    {
        var doctorClinic = await GetDoctorClinicByCompositeKey(doctorId, clinicId, specializationId);
        _set.Remove(doctorClinic);

        var result = await _context.SaveChangesAsync();

        if (result > 0)
        {
            return true;
        }
        return false;
    }
}
