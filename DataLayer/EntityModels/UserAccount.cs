using Common;

namespace DataLayer;

public partial class UserAccount
{
    public long Id { get; set; }

    public UserType UserType { get; set; }

    public string Email { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string Password { get; set; } = null!;

    public virtual Admin? Admin { get; set; }

    public virtual Doctor? Doctor { get; set; }

    public virtual MedicalStaff? MedicalStaff { get; set; }

    public virtual Patient? Patient { get; set; }
}
