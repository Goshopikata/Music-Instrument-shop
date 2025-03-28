namespace MusicShop.Services.Instruments.Models
{
    public class LatestInstrumentServiceModel : IInstrumentModel
    {
        public int Id { get; init; }

        public string Brand { get; init; }

        public string Model { get; init; }

        public string ImageUrl { get; init; }

        public int Year { get; init; }
    }
}
