using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer;

public partial class UserAccount : IdentityUser<long>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime CreatedDate { get; set; }

    public virtual Admin? Admin { get; set; }
    public virtual Patient? Patient { get; set; }
    public virtual MedicalEmployee? MedicalEmployee { get; set; }
    public virtual Doctor? Doctor { get; set; } 
}
