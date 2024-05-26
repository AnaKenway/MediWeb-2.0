using DataLayer;
using Services;

namespace MediWeb.Services;

public class MedicalEmployeeService : BaseService<MedicalEmployee>
{
    public MedicalEmployeeService(MediWebContext context)
        : base(context)
    {
    }
}
