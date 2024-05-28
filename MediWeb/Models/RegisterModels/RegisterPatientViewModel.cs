using Common;
using System.ComponentModel.DataAnnotations;

namespace MediWeb.Models;

public class RegisterPatientViewModel : BaseRegisterViewModel
{
    [Required]
    public string Jmbg { get; set; }

    [Required]
    public Gender Gender { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [Required]
    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; }  
}
