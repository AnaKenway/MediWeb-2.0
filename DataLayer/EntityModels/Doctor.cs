

namespace DataLayer;

public partial class Doctor
{
    public long Id { get; set; }
    public string? Title { get; set; }

    public long UserAccountId { get; set; }
    public virtual UserAccount UserAccount { get; set; } = null!;

    public virtual ICollection<AppointmentSlot> AppointmentSlots { get; set; } = new List<AppointmentSlot>();

    public virtual ICollection<DoctorClinics> DoctorClinics { get; set; } = new List<DoctorClinics>();
    public virtual ICollection<Clinic> Clinics { get; set; } = [];
    public virtual ICollection<Specialization> Specializations { get; set; } = [];
}
