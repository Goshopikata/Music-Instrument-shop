namespace MusicShop.Models.Instruments
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using MusicShop.Services.Instruments.Models;

    public class AllInstrumentsQueryModels
    {
        public const int InstrumentsPerPage = 3;

        public string Brand { get; init; }

        [Display(Name = "Search by text")]
        public string SearchTerm { get; init; }

        public InstrumentSorting Sorting { get; init; }

        public int CurrentPage { get; init; } = 1;

        public int TotalInstruments { get; set; }

        public IEnumerable<string> Brands { get; set; }

        public IEnumerable<InstrumentServiceModel> Instruments { get; set; }
    }
}
