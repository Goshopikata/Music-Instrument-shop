using Microsoft.AspNetCore.Mvc;

namespace MusicShop.Controllers
{
    public class MusicSchoolController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
