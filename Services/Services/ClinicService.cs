using DataLayer;
using Services;

namespace MediWeb.Services;

public class ClinicService : BaseService<Clinic>
{
    public ClinicService(MediWebContext context)
        : base(context)
    {
    }
}
