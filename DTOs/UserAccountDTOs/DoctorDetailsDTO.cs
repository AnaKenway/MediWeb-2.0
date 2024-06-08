
namespace DTOs.UserAccountDTOs
{
    public class DoctorDetailsDTO
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }   
        public long ClinicId { get; set; }
        public long SpecializationId { get; set; }
    }
}
