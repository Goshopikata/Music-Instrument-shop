using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MusicShop.Controllers;
using MusicShop.Services.Instruments;
using MusicShop.Services.Instruments.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace MusicShop.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTests
    {
        private HomeController _controller;
        private FakeInstrumentsService _fakeInstrumentService;
        private IMemoryCache _memoryCache;

        [SetUp]
        public void Setup()
        {
            _fakeInstrumentService = new FakeInstrumentsService();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _controller = new HomeController(_fakeInstrumentService, _memoryCache);
        }

        [TearDown]
        public void TearDown()
        {
            if (_controller is IDisposable disposable)
            {
                disposable.Dispose();
            }

            _memoryCache.Dispose();
        }


        [Test]
        public void Index_WhenCacheIsEmpty_CallsServiceAndReturnsData()
        {
            var result = _controller.Index() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<LatestInstrumentServiceModel>>(result.Model);

            var model = (List<LatestInstrumentServiceModel>)result.Model;
            Assert.AreEqual(1, model.Count);
            Assert.AreEqual("Yamaha", model[0].Brand);

            var cached = _memoryCache.Get<List<LatestInstrumentServiceModel>>(WebConstants.LatestInstrumentsCacheKey);
            Assert.IsNotNull(cached);
            Assert.AreEqual("Yamaha", cached[0].Brand);
            Assert.AreEqual(1, _fakeInstrumentService.LatestCalledCount);
        }

        [Test]
        public void Index_WhenCacheHasData_DoesNotCallService()
        {
            var cachedData = new List<LatestInstrumentServiceModel>
            {
                new LatestInstrumentServiceModel { Id = 2, Brand = "Fender", Model = "Strat" }
            };

            _memoryCache.Set(WebConstants.LatestInstrumentsCacheKey, cachedData);

            var result = _controller.Index() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<LatestInstrumentServiceModel>>(result.Model);

            var model = (List<LatestInstrumentServiceModel>)result.Model;
            Assert.AreEqual(1, model.Count);
            Assert.AreEqual("Fender", model[0].Brand);
            Assert.AreEqual(0, _fakeInstrumentService.LatestCalledCount);
        }

        [Test]
        public void Error_ReturnsView()
        {
            var result = _controller.Error() as ViewResult;
            Assert.IsNotNull(result);
        }

        [Test]
        public void Info_ReturnsView()
        {
            var result = _controller.Info() as ViewResult;
            Assert.IsNotNull(result);
        }

        [Test]
        public void ExtraInfo_ReturnsView()
        {
            var result = _controller.ExtraInfo() as ViewResult;
            Assert.IsNotNull(result);
        }

        private class FakeInstrumentsService : IInstrumentsService
        {
            public int LatestCalledCount { get; private set; } = 0;

            public IEnumerable<LatestInstrumentServiceModel> Latest()
            {
                LatestCalledCount++;
                return new List<LatestInstrumentServiceModel>
                {
                    new LatestInstrumentServiceModel
                    {
                        Id = 1,
                        Brand = "Yamaha",
                        Model = "FG800"
                    }
                };
            }

            public IEnumerable<string> AllBrands() => throw new NotImplementedException();
            public IEnumerable<InstrumentCategoryServiceModel> AllCategories() => throw new NotImplementedException();
            public InstrumentQueryServiceModel All(string brand = null, string searchTerm = null, InstrumentSorting sorting = InstrumentSorting.DateCreated, int currentPage = 1, int instrumentsPerPage = int.MaxValue, bool publicOnly = true) => throw new NotImplementedException();
            public IEnumerable<InstrumentServiceModel> ByUser(string userId) => throw new NotImplementedException();
            public bool CategoryExists(int categoryId) => throw new NotImplementedException();
            public int Create(string brand, string model, string description, string imageUrl, int year, int categoryId, int dealerId) => throw new NotImplementedException();
            public InstrumentDetailsServiceModel Details(int id) => throw new NotImplementedException();
            public bool Edit(int id, string brand, string model, string description, string imageUrl, int year, int categoryId, bool isPublic) => throw new NotImplementedException();
            public bool IsByDealer(int id, int dealerId) => throw new NotImplementedException();
            public void ChangeVisility(int id) => throw new NotImplementedException();
        }
    }
}
