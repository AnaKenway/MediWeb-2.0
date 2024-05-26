
namespace DataLayer;

public partial class DoctorWorksAtClinic
{
    public long DoctorId { get; set; }

    public long ClinicId { get; set; }

    public long SpecializationId { get; set; }

    public string? Note { get; set; }

    public virtual Clinic Clinic { get; set; } = null!;

    public virtual Doctor Doctor { get; set; } = null!;

    public virtual Specialization Specialization { get; set; } = null!;
}
