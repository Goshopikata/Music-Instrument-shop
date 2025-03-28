namespace MusicShop.Services.Instruments.Models
{
    public interface IInstrumentModel
    {
        string Brand { get; }

        string Model { get; }

        int Year { get; }
    }
}
