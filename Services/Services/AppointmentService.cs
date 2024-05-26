using DataLayer;
using Services;

namespace MediWeb.Services;

public class AppointmentService : BaseService<Appointment>
{
    public AppointmentService(MediWebContext context)
        : base(context)
    {
    }
   
}
