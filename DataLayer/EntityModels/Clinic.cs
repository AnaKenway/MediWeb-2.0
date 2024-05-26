
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

    public virtual ICollection<MedicalEmployee> MedicalEmployees { get; set; } = new List<MedicalEmployee>();

    public virtual ICollection<DoctorClinics> DoctorClinics { get; set; } = [];
    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    
}
