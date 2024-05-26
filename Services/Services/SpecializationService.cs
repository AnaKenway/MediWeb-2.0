using DataLayer;
using Services;

namespace MediWeb.Services;

public class SpecializationService : BaseService<Specialization>
{
    public SpecializationService(MediWebContext context)
        : base(context)
    {
    }
}
