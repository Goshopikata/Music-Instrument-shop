namespace MusicShop.Services.Dealers
{
    using MusicShop.Data;
    using System.Linq;


    public class DealerService : IDealerService
    {
        private readonly RentalDbContext data;

        public DealerService(RentalDbContext data)
            => this.data = data;

        public bool IsDealer(string userId)
            => data
                .Dealers
                .Any(d => d.UserId == userId);

        public int IdByUser(string userId)
            => data
                .Dealers
                .Where(d => d.UserId == userId)
                .Select(d => d.Id)
                .FirstOrDefault();
    }
}
