namespace MusicShop.Services.Cars
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using MusicShop.Data;
    using MusicShop.Data.Models;
    using MusicShop.Services.Cars.Models;

    public class InstrumentService : IInstrumentsService
    {
        private readonly RentalDbContext _context;
        private readonly IConfigurationProvider _mapperConfig;

        public InstrumentService(RentalDbContext context, IMapper mapper)
        {
            _context = context;
            _mapperConfig = mapper.ConfigurationProvider;
        }

        public CarQueryServiceModel All(
            string brand = null,
            string searchTerm = null,
            InstrumentSorting sorting = InstrumentSorting.DateCreated,
            int currentPage = 1,
            int instrumentsPerPage = int.MaxValue,
            bool publicOnly = true)
        {
            var carQuery = _context.Cars.AsQueryable();

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

            var totalCars = carQuery.Count();

            var instruments = GetCars(carQuery.Skip((currentPage - 1) * instrumentsPerPage).Take(instrumentsPerPage));

            return new CarQueryServiceModel
            {
                TotalCars = totalCars,
                CurrentPage = currentPage,
                CarsPerPage = instrumentsPerPage,
                Cars = instruments
            };
        }

        public IEnumerable<LatestCarServiceModel> Latest()
        {
            return _context.Cars
                .Where(c => c.IsPublic)
                .OrderByDescending(c => c.Id)
                .ProjectTo<LatestCarServiceModel>(_mapperConfig)
                .Take(3)
                .ToList();
        }

        public CarDetailsServiceModel Details(int id)
        {
            return _context.Cars
                .Where(c => c.Id == id)
                .ProjectTo<CarDetailsServiceModel>(_mapperConfig)
                .FirstOrDefault();
        }

        public int Create(string brand, string model, string description, string imageUrl, int year, int categoryId, int dealerId)
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
                IsPublic = false
            };

            _context.Cars.Add(car);
            _context.SaveChanges();

            return car.Id;
        }

        public bool Edit(int id, string brand, string model, string description, string imageUrl, int year, int categoryId, bool isPublic)
        {
            var car = _context.Cars.Find(id);

            if (car == null)
                return false;

            car.Brand = brand;
            car.Model = model;
            car.Description = description;
            car.ImageUrl = imageUrl;
            car.Year = year;
            car.CategoryId = categoryId;
            car.IsPublic = isPublic;

            _context.SaveChanges();

            return true;
        }

        public IEnumerable<CarServiceModel> ByUser(string userId)
        {
            return GetCars(_context.Cars.Where(c => c.Dealer.UserId == userId));
        }

        public bool IsByDealer(int carId, int dealerId)
        {
            return _context.Cars.Any(c => c.Id == carId && c.DealerId == dealerId);
        }


        public IEnumerable<string> AllBrands()
        {
            return _context.Cars
                .Select(c => c.Brand)
                .Distinct()
                .OrderBy(brand => brand)
                .ToList();
        }

        public IEnumerable<CarCategoryServiceModel> AllCategories()
        {
            return _context.Categories
                .ProjectTo<CarCategoryServiceModel>(_mapperConfig)
                .ToList();
        }

        public bool CategoryExists(int categoryId)
        {
            return _context.Categories.Any(c => c.Id == categoryId);
        }

        private IEnumerable<CarServiceModel> GetCars(IQueryable<Instrument> carQuery)
        {
            return carQuery.ProjectTo<CarServiceModel>(_mapperConfig).ToList();
        }

        public void ChangeVisility(int carId)
        {
            var car = _context.Cars.Find(carId);

            if (car != null)
            {
                car.IsPublic = !car.IsPublic;
                _context.SaveChanges();
            }
        }
    }
}
