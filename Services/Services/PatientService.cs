using DataLayer;
using Microsoft.EntityFrameworkCore;
using Services;

namespace MediWeb.Services;

public class PatientService : BaseService<Patient>
{
    public PatientService(MediWebContext context)
        : base(context)
    {
    }

    public async override Task<IList<Patient>> GetAllAsync()
    {
        return await _set.Include(d => d.UserAccount)
            .Include(p => p.Appointments)
            .ToListAsync();
    }

}
