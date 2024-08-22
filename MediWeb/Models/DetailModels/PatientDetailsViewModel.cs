using Common;
using DataLayer;

namespace MediWeb.Models;

public class PatientDetailsViewModel
{
    public PatientDetailsViewModel(Patient patient)
    {
        Id = patient.Id;
        FirstName = patient.UserAccount.FirstName;
        LastName = patient.UserAccount.LastName;
        Jmbg = patient.Jmbg;
        Gender = patient.Gender;
        DateOfBirth = patient.DateOfBirth;
        Email = patient.UserAccount.Email;
        PhoneNumber = patient.PhoneNumber;
    }

    public long Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Jmbg { get; set; } = null!;
    public Gender Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; } = null!;
}
