using Moq;
using NUnit.Framework;
using MusicShop.Data;
using MusicShop.Data.Models;
using MusicShop.Services.Dealers;
using MusicShop.Services.Instruments.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace MusicShop.Tests.Services
{
    [TestFixture]
    public class DealerServiceTests
    {
        private DealerService dealerService;
        private Mock<RentalDbContext> dbContextMock;

        [SetUp]
        public void SetUp()
        {
            dbContextMock = new Mock<RentalDbContext>();
            dealerService = new DealerService(dbContextMock.Object);
        }

        [Test]
        public void IsDealer_ShouldReturnTrue_WhenUserIsDealer()
        {
            // Arrange
            var userId = "test-user-id";
            var dealers = new List<Dealer>
            {
                new Dealer { UserId = userId }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Dealer>>();
            mockSet.As<IQueryable<Dealer>>().Setup(m => m.Provider).Returns(dealers.Provider);
            mockSet.As<IQueryable<Dealer>>().Setup(m => m.Expression).Returns(dealers.Expression);
            mockSet.As<IQueryable<Dealer>>().Setup(m => m.ElementType).Returns(dealers.ElementType);
            mockSet.As<IQueryable<Dealer>>().Setup(m => m.GetEnumerator()).Returns(dealers.GetEnumerator());

            dbContextMock.Setup(db => db.Dealers).Returns(mockSet.Object);

            // Act
            var result = dealerService.IsDealer(userId);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsDealer_ShouldReturnFalse_WhenUserIsNotDealer()
        {
            // Arrange
            var userId = "test-user-id";
            var dealers = new List<Dealer>().AsQueryable();

            var mockSet = new Mock<DbSet<Dealer>>();
            mockSet.As<IQueryable<Dealer>>().Setup(m => m.Provider).Returns(dealers.Provider);
            mockSet.As<IQueryable<Dealer>>().Setup(m => m.Expression).Returns(dealers.Expression);
            mockSet.As<IQueryable<Dealer>>().Setup(m => m.ElementType).Returns(dealers.ElementType);
            mockSet.As<IQueryable<Dealer>>().Setup(m => m.GetEnumerator()).Returns(dealers.GetEnumerator());

            dbContextMock.Setup(db => db.Dealers).Returns(mockSet.Object);

            // Act
            var result = dealerService.IsDealer(userId);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IdByUser_ShouldReturnDealerId_WhenUserIsDealer()
        {
            // Arrange
            var userId = "test-user-id";
            var dealerId = 1;
            var dealers = new List<Dealer>
            {
                new Dealer { Id = dealerId, UserId = userId }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Dealer>>();
            mockSet.As<IQueryable<Dealer>>().Setup(m => m.Provider).Returns(dealers.Provider);
            mockSet.As<IQueryable<Dealer>>().Setup(m => m.Expression).Returns(dealers.Expression);
            mockSet.As<IQueryable<Dealer>>().Setup(m => m.ElementType).Returns(dealers.ElementType);
            mockSet.As<IQueryable<Dealer>>().Setup(m => m.GetEnumerator()).Returns(dealers.GetEnumerator());

            dbContextMock.Setup(db => db.Dealers).Returns(mockSet.Object);

            // Act
            var result = dealerService.IdByUser(userId);

            // Assert
            Assert.AreEqual(dealerId, result);
        }

        [Test]
        public void IdByUser_ShouldThrowException_WhenUserIsNotDealer()
        {
            // Arrange
            var userId = "test-user-id";
            var dealers = new List<Dealer>().AsQueryable();

            var mockSet = new Mock<DbSet<Dealer>>();
            mockSet.As<IQueryable<Dealer>>().Setup(m => m.Provider).Returns(dealers.Provider);
            mockSet.As<IQueryable<Dealer>>().Setup(m => m.Expression).Returns(dealers.Expression);
            mockSet.As<IQueryable<Dealer>>().Setup(m => m.ElementType).Returns(dealers.ElementType);
            mockSet.As<IQueryable<Dealer>>().Setup(m => m.GetEnumerator()).Returns(dealers.GetEnumerator());

            dbContextMock.Setup(db => db.Dealers).Returns(mockSet.Object);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => dealerService.IdByUser(userId));
        }
    }

    [TestFixture]
    public class LatestInstrumentServiceModelTests
    {
        [Test]
        public void LatestInstrumentServiceModel_ShouldReturnCorrectProperties()
        {
            // Arrange
            var latestInstrument = new LatestInstrumentServiceModel
            {
                Id = 1,
                Brand = "Yamaha",
                Model = "C40",
                ImageUrl = "http://example.com/image.jpg",
                Year = 2020
            };

            // Act & Assert
            Assert.AreEqual(1, latestInstrument.Id);
            Assert.AreEqual("Yamaha", latestInstrument.Brand);
            Assert.AreEqual("C40", latestInstrument.Model);
            Assert.AreEqual("http://example.com/image.jpg", latestInstrument.ImageUrl);
            Assert.AreEqual(2020, latestInstrument.Year);
        }
    }

    [TestFixture]
    public class InstrumentSortingTests
    {
        [Test]
        public void InstrumentSorting_ShouldHaveCorrectValues()
        {
            // Assert
            Assert.AreEqual(0, (int)InstrumentSorting.DateCreated);
            Assert.AreEqual(1, (int)InstrumentSorting.Year);
            Assert.AreEqual(2, (int)InstrumentSorting.BrandAndModel);
        }
    }

    [TestFixture]
    public class InstrumentModelTests
    {
        [Test]
        public void InstrumentModel_ShouldReturnCorrectBrand()
        {
            // Arrange
            var mockInstrumentModel = new Mock<IInstrumentModel>();
            mockInstrumentModel.Setup(i => i.Brand).Returns("Yamaha");

            // Act
            var brand = mockInstrumentModel.Object.Brand;

            // Assert
            Assert.AreEqual("Yamaha", brand);
        }

        [Test]
        public void InstrumentModel_ShouldReturnCorrectModel()
        {
            // Arrange
            var mockInstrumentModel = new Mock<IInstrumentModel>();
            mockInstrumentModel.Setup(i => i.Model).Returns("C40");

            // Act
            var model = mockInstrumentModel.Object.Model;

            // Assert
            Assert.AreEqual("C40", model);
        }

        [Test]
        public void InstrumentModel_ShouldReturnCorrectYear()
        {
            // Arrange
            var mockInstrumentModel = new Mock<IInstrumentModel>();
            mockInstrumentModel.Setup(i => i.Year).Returns(2020);

            // Act
            var year = mockInstrumentModel.Object.Year;

            // Assert
            Assert.AreEqual(2020, year);
        }
    }

    [TestFixture]
    public class InstrumentCategoryServiceModelTests
    {
        [Test]
        public void InstrumentCategoryServiceModel_ShouldReturnCorrectId()
        {
            // Arrange
            var category = new InstrumentCategoryServiceModel
            {
                Id = 1,
                Name = "Guitars"
            };

            // Act
            var id = category.Id;

            // Assert
            Assert.AreEqual(1, id);
        }

        [Test]
        public void InstrumentCategoryServiceModel_ShouldReturnCorrectName()
        {
            // Arrange
            var category = new InstrumentCategoryServiceModel
            {
                Id = 1,
                Name = "Guitars"
            };

            // Act
            var name = category.Name;

            // Assert
            Assert.AreEqual("Guitars", name);
        }
    }

    [TestFixture]
    public class InstrumentDetailsServiceModelTests
    {
        [Test]
        public void InstrumentDetailsServiceModel_ShouldReturnCorrectProperties()
        {
            // Arrange
            var details = new InstrumentDetailsServiceModel
            {
                Id = 1,
                Brand = "Yamaha",
                Model = "C40",
                Description = "A classical guitar",
                CategoryId = 2,
                DealerId = 3,
                DealerName = "John's Music",
                UserId = "user-123"
            };

            // Act & Assert
            Assert.AreEqual(1, details.Id);
            Assert.AreEqual("Yamaha", details.Brand);
            Assert.AreEqual("C40", details.Model);
            Assert.AreEqual("A classical guitar", details.Description);
            Assert.AreEqual(2, details.CategoryId);
            Assert.AreEqual(3, details.DealerId);
            Assert.AreEqual("John's Music", details.DealerName);
            Assert.AreEqual("user-123", details.UserId);
        }
    }

    [TestFixture]
    public class InstrumentQueryServiceModelTests
    {
        [Test]
        public void InstrumentQueryServiceModel_ShouldReturnCorrectProperties()
        {
            // Arrange
            var instruments = new List<InstrumentServiceModel>
            {
                new InstrumentServiceModel { Id = 1, Brand = "Yamaha", Model = "C40" },
                new InstrumentServiceModel { Id = 2, Brand = "Fender", Model = "Stratocaster" }
            };

            var queryModel = new InstrumentQueryServiceModel
            {
                CurrentPage = 1,
                InstrumentsPerPage = 10,
                TotalInstruments = 2,
                Instruments = instruments
            };

            // Act & Assert
            Assert.AreEqual(1, queryModel.CurrentPage);
            Assert.AreEqual(10, queryModel.InstrumentsPerPage);
            Assert.AreEqual(2, queryModel.TotalInstruments);
            Assert.AreEqual(instruments, queryModel.Instruments);
        }
    }

    [TestFixture]
    public class InstrumentServiceModelTests
    {
        [Test]
        public void InstrumentServiceModel_ShouldReturnCorrectProperties()
        {
            // Arrange
            var instrument = new InstrumentServiceModel
            {
                Id = 1,
                Brand = "Yamaha",
                Model = "C40",
                ImageUrl = "http://example.com/image.jpg",
                Year = 2020,
                CategoryName = "Guitars",
                IsPublic = true
            };

            // Act & Assert
            Assert.AreEqual(1, instrument.Id);
            Assert.AreEqual("Yamaha", instrument.Brand);
            Assert.AreEqual("C40", instrument.Model);
            Assert.AreEqual("http://example.com/image.jpg", instrument.ImageUrl);
            Assert.AreEqual(2020, instrument.Year);
            Assert.AreEqual("Guitars", instrument.CategoryName);
            Assert.IsTrue(instrument.IsPublic);
        }
    }
}
