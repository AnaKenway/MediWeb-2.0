
namespace DataLayer;

public partial class Specialization
{
    public long Id { get; set; }
    public string SpecializationName { get; set; } = null!;

    public virtual ICollection<DoctorClinics> DoctorClinics { get; set; } = new List<DoctorClinics>();
    public virtual ICollection<Doctor> Doctors { get; set; } = [];
}
