using Microsoft.AspNetCore.Mvc;

namespace MusicShop.Controllers
{
    public class Maintenance : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
