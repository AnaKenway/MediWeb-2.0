using Common;
using DataLayer;
using Services;

namespace MediWeb.Services;

public class AdminService : BaseService<Admin>
{ 
    public AdminService(MediWebContext context)
        :base(context)
    {
    }

    
    public async Task<Admin> ChangeAdminTypeAsync(long adminId, AdminType newAdminType)
    {
        var admin = await _set.FindAsync(adminId) ??
            throw new MediWebClientException(MediWebFeature.Administration, "Cannot change type of Admin because the Admin with Id " + adminId + " doesn't exist!");
        admin.AdminType = newAdminType;
        return await UpdateAsync(admin);
    }
}
