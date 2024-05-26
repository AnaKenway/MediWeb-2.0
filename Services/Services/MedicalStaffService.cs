using DataLayer;
using Services;

namespace MediWeb.Services;

public class MedicalStaffService : BaseService<MedicalStaff>
{
    public MedicalStaffService(MediWebContext context)
        : base(context)
    {
    }
}
