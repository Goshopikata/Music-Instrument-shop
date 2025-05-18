namespace MusicShop.Services.Instruments
{
    using System.Collections.Generic;
    using MusicShop.Services.Instruments.Models;

    public interface IInstrumentsService
    {
        InstrumentQueryServiceModel All(
            string brand = null,
            string searchTerm = null,
            InstrumentSorting sorting = InstrumentSorting.DateCreated,
            int currentPage = 1,
            int instrumentsPerPage = int.MaxValue,
            bool publicOnly = true);

        IEnumerable<LatestInstrumentServiceModel> Latest();

        InstrumentDetailsServiceModel Details(int carId);

        int Create(
            string brand,
            string model,
            string description,
            string imageUrl,
            int year,
            int categoryId,
            int dealerId, 
            decimal price);




        bool Edit(
            int carId,
            string brand,
            string model,
            string description,
            string imageUrl,
            int year,
            int categoryId,
            bool isPublic, 
            decimal price);

        IEnumerable<InstrumentServiceModel> ByUser(string userId);

        bool IsByDealer(int carId, int dealerId);

        void ChangeVisility(int carId);

        IEnumerable<string> AllBrands();

        IEnumerable<InstrumentCategoryServiceModel> AllCategories();

        bool CategoryExists(int categoryId);
    }
}
