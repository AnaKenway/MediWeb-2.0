
namespace DataLayer;

public partial class DoctorClinics
{
    public long DoctorId { get; set; }
    public virtual Doctor? Doctor { get; set; }

    public long ClinicId { get; set; }
    public virtual Clinic? Clinic { get; set; }

    public long SpecializationId { get; set; }
    public virtual Specialization? Specialization { get; set; }

    public string? Note { get; set; }     
}
