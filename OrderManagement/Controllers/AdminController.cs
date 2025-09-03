using Microsoft.AspNetCore.Mvc;

namespace OrderManagementAPI.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
