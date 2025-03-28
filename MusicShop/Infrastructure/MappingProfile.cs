namespace MusicShop.Infrastructure
{
    using AutoMapper;
    using MusicShop.Data.Models;
    using MusicShop.Services.Instruments.Models;


    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, InstrumentCategoryServiceModel>();

            CreateMap<Instrument, LatestInstrumentServiceModel>();
            CreateMap<InstrumentDetailsServiceModel, CarFormModel>();

            CreateMap<Instrument, InstrumentServiceModel>()
                .ForMember(c => c.CategoryName, cfg => cfg.MapFrom(c => c.Category.Name));

            CreateMap<Instrument, InstrumentDetailsServiceModel>()
                .ForMember(c => c.UserId, cfg => cfg.MapFrom(c => c.Dealer.UserId))
                .ForMember(c => c.CategoryName, cfg => cfg.MapFrom(c => c.Category.Name));
        }
    }
}
