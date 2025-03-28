namespace MusicShop.Services.Instruments.Models
{
    using System.Collections.Generic;

    public class CarQueryServiceModel
    {
        public int CurrentPage { get; init; }

        public int InstrumentsPerPage { get; init; }

        public int TotalInstruments { get; init; }

        public IEnumerable<CarServiceModel> Instruments { get; init; }
    }
}
