using MusicShop;

namespace MusicShop.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static WebConstants;
    using MusicShop.Infrastructure.Extensions;
    using MusicShop.Services.Cars;
    using MusicShop.Models.Cars;
    using MusicShop.Services.Dealers;

    public class MusicController : Controller
    {
        private readonly IInstrumentsService cars;
        private readonly IDealerService dealers;
        private readonly IMapper mapper;

        public MusicController(
            IInstrumentsService cars,
            IDealerService dealers,
            IMapper mapper)
        {
            this.cars = cars;
            this.dealers = dealers;
            this.mapper = mapper;
        }

        public IActionResult All([FromQuery] AllInstrumentsQueryModels query)
        {
            var queryResult = cars.All(
                query.Brand,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllInstrumentsQueryModels.CarsPerPage);

            var carBrands = cars.AllBrands();

            query.Brands = carBrands;
            query.TotalCars = queryResult.TotalCars;
            query.Instruments = queryResult.Cars;

            return View(query);
        }

        [Authorize]
        public IActionResult Mine()
        {
            var myCars = cars.ByUser(User.Id());

            return View(myCars);
        }

        public IActionResult Details(int id, string information)
        {
            var car = cars.Details(id);

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
                Categories = cars.AllCategories()
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

            if (!cars.CategoryExists(car.CategoryId))
            {
                ModelState.AddModelError(nameof(car.CategoryId), "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                car.Categories = cars.AllCategories();

                return View(car);
            }

            var carId = cars.Create(
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
