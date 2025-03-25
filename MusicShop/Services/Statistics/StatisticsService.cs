namespace MusicShop.Services.Statistics
{
    using MusicShop.Data;
    using System.Linq;


    public class StatisticsService : IStatisticsService
    {
        private readonly RentalDbContext data;

        public StatisticsService(RentalDbContext data)
            => this.data = data;

        public StatisticsServiceModel Total()
        {
            var totalCars = data.Cars.Count(c => c.IsPublic);
            var totalUsers = data.Users.Count();

            return new StatisticsServiceModel
            {
                TotalCars = totalCars,
                TotalUsers = totalUsers
            };
        }
    }
}
