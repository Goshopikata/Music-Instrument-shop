namespace MusicShop.Services.Instruments.Models
{
    public interface ICarModel
    {
        string Brand { get; }

        string Model { get; }

        int Year { get; }
    }
}
