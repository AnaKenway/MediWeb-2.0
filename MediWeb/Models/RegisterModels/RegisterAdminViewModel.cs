using Common;
using DataLayer;

namespace MediWeb.Models;

public class RegisterAdminViewModel : BaseRegisterViewModel
{
    public AdminType AdminType { get; set; }

    public Admin ToEntityModel()
    {
        var admin = new Admin();
        admin.AdminType = AdminType;
        admin.UserAccount.FirstName = FirstName;
        admin.UserAccount.LastName = LastName;
        admin.UserAccount.Email = Email;
        admin.UserAccount.UserName = Email;

        return admin;
    }
}
