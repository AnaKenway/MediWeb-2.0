using DataLayer;
using Services;

namespace MediWeb.Services;

public class PatientService : BaseService<Patient>
{
    public PatientService(MediWebContext context)
        : base(context)
    {
    }
}
