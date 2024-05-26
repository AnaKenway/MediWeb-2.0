using DataLayer;
using Services;

namespace MediWeb.Services;

public class AppointmentSlotService : BaseService<AppointmentSlot>
{
    public AppointmentSlotService(MediWebContext context)
        : base(context)
    {
    }
}
