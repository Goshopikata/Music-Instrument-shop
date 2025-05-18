namespace MusicShop.Services.Instruments.Models
{
    public class InstrumentServiceModel : IInstrumentModel
    {
        public int Id { get; init; }

        public string Brand { get; init; }

        public string Model { get; init; }

        public string ImageUrl { get; init; }

        public int Year { get; init; }

        public string CategoryName { get; init; }

        public bool IsPublic { get; init; }

        public decimal Price { get; init; }
    }
}
