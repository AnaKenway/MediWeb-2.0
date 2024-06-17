using DataLayer;
using DTOs.UserAccountDTOs;
using System.ComponentModel.DataAnnotations;

namespace MediWeb.Models;

public class RegisterDoctorViewModel : BaseRegisterViewModel
{
    [Required]
    public string Title { get; set; }

    [Required]
    public long ClinicId { get; set; }

    [Required]
    public long SpecializationId { get; set; }

    [Required]
    public string Note { get; set; }

    public DoctorDetailsDTO CreateDTOFromRegisterViewModel()
    {
        return new DoctorDetailsDTO
        {
            FirstName = this.FirstName,
            LastName = this.LastName,
            Title = this.Title,
            Email = this.Email,
            DoctorClinics = new List<DoctorClinics> 
            {
                new DoctorClinics
                {
                    ClinicId = this.ClinicId,
                    SpecializationId = this.SpecializationId,
                    Note = this.Note
                }
            }
        };
    }
}
