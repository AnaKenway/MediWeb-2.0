using Common;
using DataLayer;
using System.Numerics;

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

    public static AdminDetailsViewModel CreateViewModelFromEntityModel(Admin admin)
    {
        return new AdminDetailsViewModel
        {
            Id = admin.Id,
            FirstName = admin.UserAccount.FirstName,
            LastName = admin.UserAccount.LastName,
            Email = admin.UserAccount?.Email,
            AdminType = admin.AdminType
        };
    }

    public Admin ToAdminEntityModel()
    {
        var admin = new Admin();

        admin.Id = Id;
        admin.UserAccount.FirstName = FirstName;
        admin.UserAccount.LastName = LastName;
        admin.UserAccount.Email = Email;
        admin.UserAccount.UserName = Email;
        admin.AdminType = AdminType;

        return admin;
    }
}
