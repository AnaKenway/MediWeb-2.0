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

    public DoctorDetailsDTO CreateDTOFromViewModel()
    {
        return new DoctorDetailsDTO
        {
            FirstName = this.FirstName,
            LastName = this.LastName,
            Title = this.Title,
            ClinicId = ClinicId,
            SpecializationId = SpecializationId
        };
    }
}
