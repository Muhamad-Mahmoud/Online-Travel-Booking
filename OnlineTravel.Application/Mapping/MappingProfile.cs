using AutoMapper;
using OnlineTravel.Application.Features.Tours.GetTourById.DTOs;
using OnlineTravel.Domain.Entities.Tours;
using System.Linq;

namespace OnlineTravel.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<OnlineTravel.Domain.Entities.Tours.Tour, Features.Tours.GetTourById.DTOs.TourDetailsResponse>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Title))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.DurationDays, opt => opt.MapFrom(src => src.DurationDays))
                .ForMember(dest => dest.DurationNights, opt => opt.MapFrom(src => src.DurationNights))
                .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src => src.MainImage != null ? src.MainImage.Url : string.Empty))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                .ForMember(dest => dest.Activities, opt => opt.MapFrom(src => src.Activities))
                .ForMember(dest => dest.PriceTiers, opt => opt.MapFrom(src => src.PriceTiers))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.PriceTiers.OrderBy(p => p.Price.Amount).Select(p => new Features.Tours.GetTourById.DTOs.PriceDto { Amount = p.Price.Amount, Currency = p.Price.Currency }).FirstOrDefault()));

            CreateMap<OnlineTravel.Domain.Entities.Tours.TourActivity, Features.Tours.GetTourById.DTOs.TourActivityDto>()
                 .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image != null ? src.Image.Url : string.Empty));
            
            CreateMap<OnlineTravel.Domain.Entities.Tours.TourImage, Features.Tours.GetTourById.DTOs.TourImageDto>();

            CreateMap<OnlineTravel.Domain.Entities.Tours.TourPriceTier, Features.Tours.GetTourById.DTOs.TourPriceTierDto>();

            CreateMap<OnlineTravel.Domain.Entities.Tours.TourPriceTier, Features.Tours.GetTourById.DTOs.PriceDto>()
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Price.Amount))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Price.Currency));
        }
    }
}
