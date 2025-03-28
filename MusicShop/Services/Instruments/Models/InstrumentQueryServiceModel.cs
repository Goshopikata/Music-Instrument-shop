namespace MusicShop.Services.Instruments.Models
{
    using System.Collections.Generic;

    public class InstrumentQueryServiceModel
    {
        public int CurrentPage { get; init; }

        public int InstrumentsPerPage { get; init; }

        public int TotalInstruments { get; init; }

        public IEnumerable<InstrumentServiceModel> Instruments { get; init; }
    }
}
