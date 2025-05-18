namespace MusicShop.Services.Instruments
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using MusicShop.Data;
    using MusicShop.Data.Models;
    using MusicShop.Services.Instruments.Models;

    public class InstrumentService : IInstrumentsService
    {
        private readonly RentalDbContext _context;
        private readonly IConfigurationProvider _mapperConfig;

        public InstrumentService(RentalDbContext context, IMapper mapper)
        {
            _context = context;
            _mapperConfig = mapper.ConfigurationProvider;
        }

        public InstrumentQueryServiceModel All(
            string brand = null,
            string searchTerm = null,
            InstrumentSorting sorting = InstrumentSorting.DateCreated,
            int currentPage = 1,
            int instrumentsPerPage = int.MaxValue,
            bool publicOnly = true)
        {
            var carQuery = _context.Instruments.AsQueryable();

            if (publicOnly)
                carQuery = carQuery.Where(c => c.IsPublic);

            if (!string.IsNullOrWhiteSpace(brand))
                carQuery = carQuery.Where(c => c.Brand == brand);

            if (!string.IsNullOrWhiteSpace(searchTerm))
                carQuery = carQuery.Where(c =>
                    (c.Brand + " " + c.Model).ToLower().Contains(searchTerm.ToLower()) ||
                    c.Description.ToLower().Contains(searchTerm.ToLower()));

            if (sorting == InstrumentSorting.Year)
            {
                carQuery = carQuery.OrderByDescending(c => c.Year);
            }
            else if (sorting == InstrumentSorting.BrandAndModel)
            {
                carQuery = carQuery.OrderBy(c => c.Brand).ThenBy(c => c.Model);
            }
            else
            {
                carQuery = carQuery.OrderByDescending(c => c.Id);
            }

            var totalInstruments = carQuery.Count();

            var instruments = GetInstruments(carQuery.Skip((currentPage - 1) * instrumentsPerPage).Take(instrumentsPerPage));

            return new InstrumentQueryServiceModel
            {
                TotalInstruments = totalInstruments,
                CurrentPage = currentPage,
                InstrumentsPerPage = instrumentsPerPage,
                Instruments = instruments
            };
        }

        public IEnumerable<LatestInstrumentServiceModel> Latest()
        {
            return _context.Instruments
                .Where(c => c.IsPublic)
                .OrderByDescending(c => c.Id)
                .ProjectTo<LatestInstrumentServiceModel>(_mapperConfig)
                .Take(3)
                .ToList();
        }

        public InstrumentDetailsServiceModel Details(int id)
        {
            return _context.Instruments
                .Where(c => c.Id == id)
                .ProjectTo<InstrumentDetailsServiceModel>(_mapperConfig)
                .FirstOrDefault();
        }

        public int Create(string brand, string model, string description, string imageUrl, int year, int categoryId, int dealerId, decimal price)
        {
            var car = new Instrument
            {
                Brand = brand,
                Model = model,
                Description = description,
                ImageUrl = imageUrl,
                Year = year,
                CategoryId = categoryId,
                DealerId = dealerId,
                IsPublic = false,
                Price = price
            };

            _context.Instruments.Add(car);
            _context.SaveChanges();

            return car.Id;
        }

        public bool Edit(int id, string brand, string model, string description, string imageUrl, int year, int categoryId, bool isPublic, decimal price)
        {
            var car = _context.Instruments.Find(id);

            if (car == null)
                return false;

            car.Brand = brand;
            car.Model = model;
            car.Description = description;
            car.ImageUrl = imageUrl;
            car.Year = year;
            car.CategoryId = categoryId;
            car.IsPublic = isPublic;
            car.Price = price;

            _context.SaveChanges();

            return true;
        }

        public IEnumerable<InstrumentServiceModel> ByUser(string userId)
        {
            return GetInstruments(_context.Instruments.Where(c => c.Dealer.UserId == userId));
        }

        public bool IsByDealer(int carId, int dealerId)
        {
            return _context.Instruments.Any(c => c.Id == carId && c.DealerId == dealerId);
        }


        public IEnumerable<string> AllBrands()
        {
            return _context.Instruments
                .Select(c => c.Brand)
                .Distinct()
                .OrderBy(brand => brand)
                .ToList();
        }

        public IEnumerable<InstrumentCategoryServiceModel> AllCategories()
        {
            return _context.Categories
                .ProjectTo<InstrumentCategoryServiceModel>(_mapperConfig)
                .ToList();
        }

        public bool CategoryExists(int categoryId)
        {
            return _context.Categories.Any(c => c.Id == categoryId);
        }

        private IEnumerable<InstrumentServiceModel> GetInstruments(IQueryable<Instrument> carQuery)
        {
            return carQuery.ProjectTo<InstrumentServiceModel>(_mapperConfig).ToList();
        }

        public void ChangeVisility(int carId)
        {
            var car = _context.Instruments.Find(carId);

            if (car != null)
            {
                car.IsPublic = !car.IsPublic;
                _context.SaveChanges();
            }
        }
    }
}
