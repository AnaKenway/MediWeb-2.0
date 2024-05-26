
namespace DataLayer;

public partial class Clinic
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string? Email { get; set; }
    public string? Pib { get; set; }
    public string? WorkHours { get; set; }
    public string? PhoneNumber { get; set; }


    public virtual ICollection<AppointmentSlot> AppointmentSlots { get; set; } = new List<AppointmentSlot>();  

    public virtual ICollection<MedicalStaff> MedicalStaff { get; set; } = new List<MedicalStaff>();

    public virtual ICollection<Doctor> DoctorS { get; set; } = new List<Doctor>();
}
