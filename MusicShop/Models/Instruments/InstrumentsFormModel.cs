namespace MusicShop.Models.Instruments
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using MusicShop.Services.Instruments.Models;

    using static MusicShop.Data.DataConstants.Car;

    public class InstrumentsFormModel : IInstrumentModel
    {
        [Required(ErrorMessage = "Brand is mandatory.")]
        [StringLength(BrandMaxLength, MinimumLength = BrandMinLength, ErrorMessage = "Brand length should be between {2} and {1} characters.")]
        public string Brand { get; init; }

        [Required(ErrorMessage = "Model is mandatory.")]
        [StringLength(ModelMaxLength, MinimumLength = ModelMinLength, ErrorMessage = "Model length should be between {2} and {1} characters.")]
        public string Model { get; init; }

        [Required(ErrorMessage = "Description is mandatory.")]
        [StringLength(int.MaxValue, MinimumLength = DescriptionMinLength, ErrorMessage = "Description must be at least {2} characters long.")]
        public string Description { get; init; }

        [Required(ErrorMessage = "Image URL is mandatory.")]
        [Url(ErrorMessage = "Please enter a valid URL.")]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; init; }

        [Range(YearMinValue, YearMaxValue, ErrorMessage = "Year must be between {1} and {2}.")]
        public int Year { get; init; }

        public int CategoryId { get; init; }

        public IEnumerable<InstrumentCategoryServiceModel> Categories { get; set; } = new List<InstrumentCategoryServiceModel>();
    }
}
