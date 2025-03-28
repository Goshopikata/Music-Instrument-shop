using MusicShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using MusicShop;
using MusicShop.Services.Instruments.Models;
using MusicShop.Services.Instruments;

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
            var latestInstruments = cache.Get<List<LatestInstrumentServiceModel>>(WebConstants.LatestInstrumentsCacheKey);

            if (latestInstruments == null)
            {
                latestInstruments = instruments
                   .Latest()
                   .ToList();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

                cache.Set(WebConstants.LatestInstrumentsCacheKey, latestInstruments, cacheOptions);
            }

            return View(latestInstruments);
        }

        public IActionResult Error() => View();
        public IActionResult Info() => View();
        public IActionResult ExtraInfo() => View();
    }
}