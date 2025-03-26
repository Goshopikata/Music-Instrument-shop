namespace MusicShop.Infrastructure
{
    using AutoMapper;
    using MusicShop.Data.Models;
    using MusicShop.Services.Cars.Models;


    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CarCategoryServiceModel>();

            CreateMap<Instrument, LatestCarServiceModel>();
            CreateMap<CarDetailsServiceModel, CarFormModel>();

            CreateMap<Instrument, CarServiceModel>()
                .ForMember(c => c.CategoryName, cfg => cfg.MapFrom(c => c.Category.Name));

            CreateMap<Instrument, CarDetailsServiceModel>()
                .ForMember(c => c.UserId, cfg => cfg.MapFrom(c => c.Dealer.UserId))
                .ForMember(c => c.CategoryName, cfg => cfg.MapFrom(c => c.Category.Name));
        }
    }
}
