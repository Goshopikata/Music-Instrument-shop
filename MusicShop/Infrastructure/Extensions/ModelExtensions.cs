namespace MusicShop.Infrastructure.Extensions
{
    using MusicShop.Services.Instruments.Models;

    public static class ModelExtensions
    {
        public static string GetInformation(this ICarModel car)
            => car.Brand + "-" + car.Model + "-" + car.Year;
    }
}
