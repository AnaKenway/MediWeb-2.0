using DataLayer;
using Services;

namespace MediWeb.Services;

public class UserAccountService : BaseService<UserAccount>
{
    public UserAccountService(MediWebContext context)
        : base(context)
    {
    }
}
