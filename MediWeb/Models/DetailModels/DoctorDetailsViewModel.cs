using DataLayer;
using DTOs.UserAccountDTOs;

namespace MediWeb.Models;

public class DoctorDetailsViewModel
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string ClinicName { get; set; }
    public long ClinicId { get; set; }

    public ICollection<AppointmentSlot> AppointmentSlots { get; set; } = new List<AppointmentSlot>();

    public ICollection<DoctorClinics> DoctorClinics { get; set; } = new List<DoctorClinics>();
    public ICollection<Clinic> Clinics { get; set; } = [];
    public ICollection<Specialization> Specializations { get; set; } = [];

    public string FullName { get => FirstName + " " + LastName; }

    public static DoctorDetailsViewModel CreateViewModelFromEntityModel(Doctor doctor)
    {
        return new DoctorDetailsViewModel
        {
            Id = doctor.Id,
            FirstName = doctor.UserAccount.FirstName,
            LastName = doctor.UserAccount.LastName,
            Email = doctor.UserAccount.Email,
        };
    }

    public DoctorDetailsDTO CreateDTOFromDetailsViewModel()
    {
        return new DoctorDetailsDTO
        {
            Id = this.Id,
            FirstName = this.FirstName,
            LastName = this.LastName,
            Email = this.Email,
            ClinicId = this.ClinicId,

        };
    }
}
