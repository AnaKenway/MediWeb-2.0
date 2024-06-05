using Common;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using Services;

namespace MediWeb.Services;

public class MedicalEmployeeService : BaseService<MedicalEmployee>
{
    public MedicalEmployeeService(MediWebContext context)
        : base(context)
    {
    }
    public override async Task<IList<MedicalEmployee>> GetAllAsync()
    {
        return await _set
            .Include(me => me.Clinic)
            .Include(me => me.UserAccount)
            .ToListAsync();
    }

    public override async Task<MedicalEmployee> GetByIdAsync(long id)
    {
        id.AssertIsNotNull();
        id.AssertIsNotZero();

        return await _set
            .Include(me => me.Clinic)
            .Include(me => me.UserAccount)
            .FirstOrDefaultAsync(me => me.Id == id) ??
            throw new MediWebClientException(MediWebFeature.CRUD, "Object with given Id doesn't exist.");
    }
}
