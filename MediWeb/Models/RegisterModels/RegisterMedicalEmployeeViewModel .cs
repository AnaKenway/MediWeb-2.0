using System.ComponentModel.DataAnnotations;
using DTOs.UserAccountDTOs;

namespace MediWeb.Models;

public class RegisterMedicalEmployeeViewModel : BaseRegisterViewModel
{
    [Required]
    public long ClinicId { get; set; }

    public MedicalEmployeeDetailsDTO CreateDTOFromViewModel()
    {
        return new MedicalEmployeeDetailsDTO
        {
            FirstName = this.FirstName,
            LastName = this.LastName,
            Email = this.Email,
            ClinicId  = this.ClinicId
        };
    }
}
