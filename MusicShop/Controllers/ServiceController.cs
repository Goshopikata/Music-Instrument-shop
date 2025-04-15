using Microsoft.AspNetCore.Mvc;

namespace MusicShop.Controllers
{
    public class ServiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
