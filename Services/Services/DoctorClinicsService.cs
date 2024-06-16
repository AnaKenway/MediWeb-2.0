using DataLayer;
using Services;

namespace MediWeb.Services;

public class DoctorClinicsService : BaseService<DoctorClinics>
{
    public DoctorClinicsService(MediWebContext context)
        : base(context)
    {
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
}
