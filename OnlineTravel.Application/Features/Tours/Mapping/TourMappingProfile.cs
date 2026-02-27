using AutoMapper;
using OnlineTravel.Application.Features.Tours.GetTourById.DTOs;
using OnlineTravel.Domain.Entities.Tours;

namespace OnlineTravel.Application.Features.Tours.Mapping
{
    public class TourMappingProfile : Profile
    {
        public TourMappingProfile()
        {
            CreateMap<Tour, TourDetailsResponse>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Title))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.DurationDays, opt => opt.MapFrom(src => src.DurationDays))
                .ForMember(dest => dest.DurationNights, opt => opt.MapFrom(src => src.DurationNights))
                .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src => src.MainImage != null ? src.MainImage.Url : string.Empty))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                .ForMember(dest => dest.Activities, opt => opt.MapFrom(src => src.Activities))
                .ForMember(dest => dest.PriceTiers, opt => opt.MapFrom(src => src.PriceTiers))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.PriceTiers.OrderBy(p => p.Price.Amount).Select(p => new PriceDto { Amount = p.Price.Amount, Currency = p.Price.Currency }).FirstOrDefault()));

            CreateMap<TourActivity, TourActivityDto>()
                 .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image != null ? src.Image.Url : string.Empty));

            CreateMap<TourImage, TourImageDto>();

            CreateMap<TourPriceTier, TourPriceTierDto>();

            CreateMap<TourPriceTier, PriceDto>()
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Price.Amount))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Price.Currency));
        }
    }
}
