
namespace DataLayer;

public partial class MedicalStaff
{
    public long Id { get; set; }

    public long ClinicId { get; set; }

    public long UserAccountId { get; set; }

    public virtual Clinic Clinic { get; set; } = null!;

    public virtual UserAccount UserAccount { get; set; } = null!;
}
