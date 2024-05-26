using DataLayer;
using Services;

namespace MediWeb.Services;

public class DoctorService : BaseService<Doctor>
{
    public DoctorService(MediWebContext context)
        : base(context)
    {
    }
}
