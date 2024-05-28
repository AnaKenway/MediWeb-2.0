using System.ComponentModel.DataAnnotations;

namespace MediWeb.Models;

public class RegisterMedicalEmployeeViewModel : BaseRegisterViewModel
{
    [Required]
    public long ClinicId { get; set; }
}
