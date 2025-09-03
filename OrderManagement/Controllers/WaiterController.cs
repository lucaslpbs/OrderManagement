using Microsoft.AspNetCore.Mvc;

namespace OrderManagementAPI.Controllers
{
    public class WaiterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
