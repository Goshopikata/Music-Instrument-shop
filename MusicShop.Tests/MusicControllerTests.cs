using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicShop.Controllers;
using MusicShop.Data;
using MusicShop.Data.Models;
using MusicShop.Models.Instruments;
using MusicShop.Services.Dealers;
using MusicShop.Services.Instruments;
using MusicShop.Services.Instruments.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace MusicShop.Tests.Controllers
{
    [TestFixture]
    public class MusicControllerTests
    {
        private RentalDbContext _context;
        private FakeInstrumentsService _instrumentService;
        private FakeDealerService _dealerService;
        private MusicController _controller;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<RentalDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new RentalDbContext(options);
            _instrumentService = new FakeInstrumentsService();
            _dealerService = new FakeDealerService();

            _controller = new MusicController(
                _instrumentService,
                _dealerService,
                mapper: null
            );

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user-id")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [TearDown]
        public void TearDown()
        {
            _context?.Database.EnsureDeleted();
            _context?.Dispose();
        }

        [Test]
        public void All_ReturnsViewWithCorrectModel()
        {
            var query = new AllInstrumentsQueryModels();

            var result = _controller.All(query) as ViewResult;

            Assert.IsNotNull(result);
            var model = result.Model as AllInstrumentsQueryModels;
            Assert.IsNotNull(model);
            Assert.AreEqual(0, model.TotalInstruments);
        }

        [Test]
        public void Mine_ReturnsViewWithUserInstruments()
        {
            var result = _controller.Mine() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<InstrumentServiceModel>>(result.Model);
        }

        [Test]
        public void Add_Get_WithNonDealerUserRedirectsToBecome()
        {
            _dealerService.IsDealerResult = false;

            var result = _controller.Add() as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Become", result.ActionName);
            Assert.AreEqual("Dealers", result.ControllerName);
        }

        [Test]
        public void Add_Post_InvalidCategory_ReturnsViewWithErrors()
        {
            _dealerService.DealerIdResult = 1;
            _instrumentService.CategoryExistsResult = false;

            var formModel = new InstrumentsFormModel
            {
                Brand = "Yamaha",
                Model = "FG800",
                Description = "Acoustic guitar",
                ImageUrl = "https://example.com/img.jpg",
                Year = 2022,
                CategoryId = 999
            };

            var result = _controller.Add(formModel) as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsFalse(_controller.ModelState.IsValid);
            Assert.IsInstanceOf<InstrumentsFormModel>(result.Model);
        }

        

        private class FakeInstrumentsService : IInstrumentsService
        {
            public bool CategoryExistsResult = true;

            public IEnumerable<string> AllBrands() => new List<string>();

            public InstrumentQueryServiceModel All(string brand, string searchTerm, InstrumentSorting sorting, int currentPage, int instrumentsPerPage)
                => new InstrumentQueryServiceModel { Instruments = new List<InstrumentServiceModel>(), TotalInstruments = 0 };

            public IEnumerable<InstrumentCategoryServiceModel> AllCategories() => new List<InstrumentCategoryServiceModel>();

            public IEnumerable<InstrumentServiceModel> ByUser(string userId) => new List<InstrumentServiceModel>();

            public InstrumentDetailsServiceModel Details(int id) => new InstrumentDetailsServiceModel
            {
                Id = id,
                Brand = "Test",
                Model = "Test",
                Description = "Test",
                ImageUrl = "test.jpg",
                Year = 2023,
                DealerId = 1,
                CategoryName = "Guitar"
            };

            public bool CategoryExists(int categoryId) => CategoryExistsResult;

            public int Create(string brand, string model, string description, string imageUrl, int year, int categoryId, int dealerId) => 1;
        }

        private class FakeDealerService : IDealerService
        {
            public bool IsDealerResult = true;
            public int DealerIdResult = 1;

            public bool IsDealer(string userId) => IsDealerResult;

            public int IdByUser(string userId) => DealerIdResult;
        }
    }
}
