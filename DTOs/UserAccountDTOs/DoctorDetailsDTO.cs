using DataLayer;

namespace DTOs.UserAccountDTOs;

public class DoctorDetailsDTO
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Title { get; set; }  
    public IList<DoctorClinics> DoctorClinics { get; set; } = new List<DoctorClinics>();
}
