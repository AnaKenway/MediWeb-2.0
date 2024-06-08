
namespace DTOs.UserAccountDTOs
{
    public class MedicalEmployeeDetailsDTO
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ClinicName { get; set; }
        public long ClinicId { get; set; }
    }
}
