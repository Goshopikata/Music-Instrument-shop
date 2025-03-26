namespace MusicShop.Services.Cars
{
    using System.Collections.Generic;
    using MusicShop.Services.Cars.Models;

    public interface IInstrumentsService
    {
        CarQueryServiceModel All(
            string brand = null,
            string searchTerm = null,
            InstrumentSorting sorting = InstrumentSorting.DateCreated,
            int currentPage = 1,
            int instrumentsPerPage = int.MaxValue,
            bool publicOnly = true);

        IEnumerable<LatestCarServiceModel> Latest();

        CarDetailsServiceModel Details(int carId);

        int Create(
            string brand,
            string model,
            string description,
            string imageUrl,
            int year,
            int categoryId,
            int dealerId);

        bool Edit(
            int carId,
            string brand,
            string model,
            string description,
            string imageUrl,
            int year,
            int categoryId,
            bool isPublic);

        IEnumerable<CarServiceModel> ByUser(string userId);

        bool IsByDealer(int carId, int dealerId);

        void ChangeVisility(int carId);

        IEnumerable<string> AllBrands();

        IEnumerable<CarCategoryServiceModel> AllCategories();

        bool CategoryExists(int categoryId);
    }
}
