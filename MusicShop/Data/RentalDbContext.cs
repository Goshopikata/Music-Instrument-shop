namespace MusicShop.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using MusicShop.Data.Models;

    public class RentalDbContext : IdentityDbContext<User>
    {
       public RentalDbContext ()
        {

        }
        public RentalDbContext(DbContextOptions<RentalDbContext> options)
            : base(options)
        {
        }

        public DbSet<Instrument> Cars { get; init; }

        public DbSet<Category> Categories { get; init; }

        public DbSet<Dealer> Dealers { get; init; }
        public override DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MusicShopChepBate6;Trusted_Connection=True;MultipleActiveResultSets=true");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Instrument>()
                .HasOne(c => c.Category)
                .WithMany(c => c.Cars)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Instrument>()
                .HasOne(c => c.Dealer)
                .WithMany(d => d.Instrument)
                .HasForeignKey(c => c.DealerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Dealer>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<Dealer>(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
