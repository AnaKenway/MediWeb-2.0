using Common;
using DataLayer;

namespace MediWeb.Models;

public class AdminDetailsViewModel
{
    public AdminDetailsViewModel() { }

    public AdminDetailsViewModel(Admin admin)
    {
        Id = admin.Id;
        FirstName = admin.UserAccount.FirstName;
        LastName = admin.UserAccount.LastName;
        Email = admin.UserAccount.Email ?? string.Empty;
        AdminType = admin.AdminType;
    }

    public long Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; }
    public AdminType AdminType { get; set; }

    public Admin ToAdminEntityModel()
    {
        var admin = new Admin();

        admin.Id = Id;
        admin.UserAccount.FirstName = FirstName;
        admin.UserAccount.LastName = LastName;
        admin.UserAccount.Email = Email;
        admin.AdminType = AdminType;

        return admin;
    }
}
