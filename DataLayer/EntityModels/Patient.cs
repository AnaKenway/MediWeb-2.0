using Common;

namespace DataLayer;

public partial class Patient
{
    public long Id { get; set; }
    public string Jmbg { get; set; } = null!;
    public Gender Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string PhoneNumber { get; set; } = null!;

    public long UserAccountId { get; set; }
    public virtual UserAccount UserAccount { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
