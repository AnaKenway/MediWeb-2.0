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
}
