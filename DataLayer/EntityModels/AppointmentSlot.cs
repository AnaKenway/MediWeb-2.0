
namespace DataLayer;

public partial class AppointmentSlot
{
    public long Id { get; set; }    
    public DateTime DateAndTime { get; set; }
    public int DurationInMinutes { get; set; }

    public long? AppointmentId { get; set; }
    public virtual Appointment? Appointment { get; set; }

    public long ClinicId { get; set; }
    public virtual Clinic Clinic { get; set; } = null!;

    public long DoctorId { get; set; }
    public virtual Doctor Doctor { get; set; } = null!;
}
