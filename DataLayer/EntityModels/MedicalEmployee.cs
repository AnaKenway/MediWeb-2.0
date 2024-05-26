
namespace DataLayer;

public partial class MedicalEmployee
{
    public long Id { get; set; }

    public long ClinicId { get; set; }
    public virtual Clinic Clinic { get; set; } = null!;

    public long UserAccountId { get; set; }
    public virtual UserAccount UserAccount { get; set; } = null!;
}
