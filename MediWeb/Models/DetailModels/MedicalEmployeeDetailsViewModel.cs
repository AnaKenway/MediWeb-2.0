using DataLayer;
using DTOs.UserAccountDTOs;

namespace MediWeb.Models;

public class MedicalEmployeeDetailsViewModel
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string ClinicName { get; set; }
    public long ClinicId { get; set; }

    public string FullName { get => FirstName + " " + LastName; }

    public static MedicalEmployeeDetailsViewModel CreateViewModelFromEntityModel(MedicalEmployee medicalEmployee)
    {
        return new MedicalEmployeeDetailsViewModel
        {
            Id = medicalEmployee.Id,
            FirstName = medicalEmployee.UserAccount.FirstName,
            LastName = medicalEmployee.UserAccount.LastName,
            Email = medicalEmployee.UserAccount.Email,
            ClinicId = medicalEmployee.ClinicId,
            ClinicName = medicalEmployee.Clinic.Name
        };
    }

    public MedicalEmployeeDetailsDTO CreateDTOFromDetailsViewModel()
    {
        return new MedicalEmployeeDetailsDTO
        {
             Id = this.Id,
             FirstName = this.FirstName,
             LastName = this.LastName,
             Email = this.Email,
             ClinicName = this.ClinicName,
             ClinicId = this.ClinicId
        };
    }
}
