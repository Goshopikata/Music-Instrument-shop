using Microsoft.AspNetCore.Identity;
using MusicShop.Data.Models;
using System.ComponentModel.DataAnnotations;

public class WishlistItem
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; }
    public User User { get; set; }

    [Required]
    public int InstrumentId { get; set; }
    public Instrument Instrument { get; set; }

    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}
