using Microsoft.AspNetCore.Mvc;

namespace MediWeb.Controllers
{
    public class MedicalEmployeeController : Controller
    {
        //Here, we will bind and get data from RegisterMedicalEmployeeViewModel and MedicalEmployeeDetails model
        //and we convert them to the DTOs that are then sent to the Service layers, and then in the service layers,
        //we can finally transform them to the Entity model and manipulate the data
        //Simialrly when returning data, we transform from EntityModel to the DTOs first, and lastly to View model
        public IActionResult Index()
        {
            return View();
        }
    }
}
