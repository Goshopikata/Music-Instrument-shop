using MusicShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using MusicShop;
using MusicShop.Services.Cars.Models;
using MusicShop.Services.Cars;

namespace MusicShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IInstrumentsService instruments;
        private readonly IMemoryCache cache;

        public HomeController(
            IInstrumentsService instruments,
            IMemoryCache cache)
        {
            this.instruments = instruments;
            this.cache = cache;
        }

        public IActionResult Index()
        {
            var latestCars = cache.Get<List<LatestCarServiceModel>>(WebConstants.LatestCarsCacheKey);

            if (latestCars == null)
            {
                latestCars = instruments
                   .Latest()
                   .ToList();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

                cache.Set(WebConstants.LatestCarsCacheKey, latestCars, cacheOptions);
            }

            return View(latestCars);
        }

        public IActionResult Error() => View();
        public IActionResult Info() => View();
        public IActionResult ExtraInfo() => View();
    }
}