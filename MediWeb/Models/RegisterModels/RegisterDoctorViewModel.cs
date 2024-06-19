using DataLayer;
using DTOs.UserAccountDTOs;
using System.ComponentModel.DataAnnotations;

namespace MediWeb.Models;

public class RegisterDoctorViewModel : BaseRegisterViewModel
{
    [Required]
    public string Title { get; set; }

    public IList<DoctorClinicsViewModel> DoctorClinics { get; set; } = new List<DoctorClinicsViewModel>();

    public DoctorDetailsDTO CreateDTOFromRegisterViewModel()
    {
        return new DoctorDetailsDTO
        {
            FirstName = this.FirstName,
            LastName = this.LastName,
            Title = this.Title,
            Email = this.Email,
            DoctorClinics = this.DoctorClinics.Select(dc => new DoctorClinics 
            { 
                ClinicId = dc.ClinicId,
                SpecializationId = dc.SpecializationId,
                Note = dc.Note
            }).ToList()
        };
    }
}
