using Microsoft.AspNetCore.Mvc;

namespace MediWeb.Controllers;

public class AdminController : Controller
{
    // GET: Admin
    public IActionResult Index()
    {
        return View();
    }
}
