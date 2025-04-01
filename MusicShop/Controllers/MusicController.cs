using MusicShop;

namespace MusicShop.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static WebConstants;
    using MusicShop.Infrastructure.Extensions;
    using MusicShop.Services.Instruments;
    using MusicShop.Models.Instruments;
    using MusicShop.Services.Dealers;

    public class MusicController : Controller
    {
        private readonly IInstrumentsService instruments;
        private readonly IDealerService dealers;
        private readonly IMapper mapper;

        public MusicController(
            IInstrumentsService instruments,
            IDealerService dealers,
            IMapper mapper)
        {
            this.instruments = instruments;
            this.dealers = dealers;
            this.mapper = mapper;
        }

        public IActionResult All([FromQuery] AllInstrumentsQueryModels query)
        {
            var queryResult = instruments.All(
                query.Brand,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllInstrumentsQueryModels.InstrumentsPerPage);

            var instrumentsBrands = instruments.AllBrands();

            query.Brands = instrumentsBrands;
            query.TotalInstruments = queryResult.TotalInstruments;
            query.Instruments = queryResult.Instruments;

            return View(query);
        }

        [Authorize]
        public IActionResult Mine()
        {
            var myInstruments = instruments.ByUser(User.Id());

            return View(myInstruments);
        }

        public IActionResult Details(int id, string information)
        {
            var car = instruments.Details(id);

            if (information != car.GetInformation())
            {
                return BadRequest();
            }

            return View(car);
        }

        [Authorize]
        public IActionResult Add()
        {
            if (!dealers.IsDealer(User.Id()))
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealers");
            }

            return View(new InstrumentsFormModel
            {
                Categories = instruments.AllCategories()
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(InstrumentsFormModel car)
        {
            var dealerId = dealers.IdByUser(User.Id());

            if (dealerId == 0)
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealers");
            }

            if (!instruments.CategoryExists(car.CategoryId))
            {
                ModelState.AddModelError(nameof(car.CategoryId), "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                car.Categories = instruments.AllCategories();

                return View(car);
            }

            var carId = instruments.Create(
                car.Brand,
                car.Model,
                car.Description,
                car.ImageUrl,
                car.Year,
                car.CategoryId,
                dealerId);

            TempData[GlobalMessageKey] = "You car was added and is awaiting for approval!";

            return RedirectToAction(nameof(Details), new { id = carId, information = car.GetInformation() });
        }
    }
}
