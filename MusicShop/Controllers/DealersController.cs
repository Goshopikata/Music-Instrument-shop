using MusicShop;

namespace MusicShop.Controllers
{
    using System.Linq;
    using MusicShop.Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static WebConstants;
    using MusicShop.Data;
    using MusicShop.Infrastructure.Extensions;
    using MusicShop.Models.Dealers;

    public class DealersController : Controller
    {
        private readonly RentalDbContext data;

        public DealersController(RentalDbContext data)
            => this.data = data;

        [Authorize]
        public IActionResult Become() => View();

        [HttpPost]
        [Authorize]
        public IActionResult Become(BecomeDealerFormModel dealer)
        {
            var userId = User.Id();

            var userIdAlreadyDealer = data
                .Dealers
                .Any(d => d.UserId == userId);

            if (userIdAlreadyDealer)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(dealer);
            }

            var dealerData = new Dealer
            {
                Name = dealer.Name,
                PhoneNumber = dealer.PhoneNumber,
                UserId = userId
            };

            data.Dealers.Add(dealerData);
            data.SaveChanges();

            TempData[GlobalMessageKey] = "Thank you for becomming a dealer!";

            return RedirectToAction(nameof(MusicController.All), "Cars");
        }
    }
}
