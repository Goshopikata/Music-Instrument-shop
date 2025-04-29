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
            var instrument = instruments.Details(id);

            if (information != instrument.GetInformation())
            {
                return BadRequest();
            }

            return View(instrument);
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
        public IActionResult Add(InstrumentsFormModel instrument)
        {
            var dealerId = dealers.IdByUser(User.Id());

            if (dealerId == 0)
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealers");
            }

            if (!instruments.CategoryExists(instrument.CategoryId))
            {
                ModelState.AddModelError(nameof(instrument.CategoryId), "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                instrument.Categories = instruments.AllCategories();

                return View(instrument);
            }

            var instrumentId = instruments.Create(
                instrument.Brand,
                instrument.Model,
                instrument.Description,
                instrument.ImageUrl,
                instrument.Year,
                instrument.CategoryId,
                dealerId);

            TempData[GlobalMessageKey] = "You instrument was added and is awaiting for approval!";

            return RedirectToAction(nameof(Details), new { id = instrumentId, information = instrument.GetInformation() });
        }

        public IActionResult ThankYou()
        {
            return View();
        }
    }
}
