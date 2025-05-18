using MusicShop.Data;

namespace MusicShop.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Car;

    public class Instrument
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(BrandMaxLength)]
        public string Brand { get; set; }

        [Required]
        [MaxLength(ModelMaxLength)]
        public string Model { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string? ImageUrl { get; set; }

        public int Year { get; set; }

        public bool IsPublic { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; init; } = null!;

        public int DealerId { get; init; }

        public decimal Price { get; set; }

        public Dealer Dealer { get; init; } = null!;
        public ICollection<WishlistItem> wishlistItems { get; set; } = new List<WishlistItem>();
    }
}
