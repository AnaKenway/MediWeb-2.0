using DataLayer;
using DTOs.UserAccountDTOs;
using System.Text;

namespace MediWeb.Models;

public class DoctorDetailsViewModel
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Title { get; set; }
    public string Email { get; set; }

    public IList<AppointmentSlot> AppointmentSlots { get; set; } = new List<AppointmentSlot>();

    public IList<DoctorClinics> DoctorClinics { get; set; } = new List<DoctorClinics>();
    public IList<Clinic> Clinics { get; set; } = [];
    public IList<Specialization> Specializations { get; set; } = [];

    public string FullName { get => FirstName + " " + LastName; }
    public string ClinicNames {  get => string.Join(",", Clinics); }

    public static DoctorDetailsViewModel CreateViewModelFromEntityModel(Doctor doctor)
    {
        return new DoctorDetailsViewModel
        {
            Id = doctor.Id,
            FirstName = doctor.UserAccount.FirstName,
            LastName = doctor.UserAccount.LastName,
            Email = doctor.UserAccount?.Email,
            Clinics = doctor.DoctorClinics.Select(dc => dc.Clinic).ToList()
        };
    }

    public DoctorDetailsDTO CreateDTOFromDetailsViewModel()
    {
        return new DoctorDetailsDTO
        {
            Id = this.Id,
            FirstName = this.FirstName,
            LastName = this.LastName,
            Title = this.Title,
            Email = this.Email,
            DoctorClinics = this.DoctorClinics
        };
    }
}
