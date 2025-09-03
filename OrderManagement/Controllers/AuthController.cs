using Microsoft.AspNetCore.Mvc;

namespace OrderManagementAPI.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
