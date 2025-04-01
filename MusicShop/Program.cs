using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static MusicShop.Data.DataConstants;
using MusicShop.Data;
using MusicShop.Services.Instruments;
using MusicShop.Infrastructure.Extensions;
using MusicShop.Services.Statistics;
using MusicShop.Services.Dealers;
using MusicShop.Controllers;

namespace MusicShop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

           
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                            throw new NullReferenceException("Connection string was null");

            builder.Services.AddDbContext<RentalDbContext>(x => x.UseSqlServer(connectionString));

            builder.Services.AddAutoMapper();

            builder.Services.AddMemoryCache();

            builder.Services.AddTransient<IInstrumentsService, InstrumentService>();
            builder.Services.AddTransient<IDealerService, DealerService>();
            builder.Services.AddTransient<IStatisticsService, StatisticsService>();

            builder.Services.AddDefaultIdentity<Data.Models.User>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<RentalDbContext>();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            app.PrepareDatabase();

           
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
               
                app.UseHsts();
            }

            app
            .UseHttpsRedirection()
            .UseStaticFiles()
            .UseRouting()
            .UseAuthentication()
            .UseAuthorization()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultAreaRoute();

                endpoints.MapControllerRoute(
                    name: "Car Details",
                    pattern: "/Instruments/Details/{id}/{information}",
                    defaults: new
                    {
                        controller = typeof(MusicController).GetControllerName(),
                        action = nameof(MusicController.Details)
                    });

                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
                           });
            app.Run();
        }
    }
}