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
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => $"{src.Address.City}, {src.Address.Country}"))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => $"{src.DurationDays} Days / {src.DurationNights} Nights"))
                .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src => src.MainImage != null ? src.MainImage.Url : string.Empty))
                .ForMember(dest => dest.Gallery, opt => opt.MapFrom(src => src.Images.Select(i => i.Url).ToList()))
                .ForMember(dest => dest.TopActivities, opt => opt.MapFrom(src => src.Activities.Take(3).ToList())) // Assuming Top 3 for summary
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.PriceTiers.FirstOrDefault())); // Simplified for now

            CreateMap<OnlineTravel.Domain.Entities.Tours.TourActivity, Features.Tours.GetTourById.DTOs.TourActivityDto>()
                 .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image != null ? src.Image.Url : string.Empty));

            CreateMap<OnlineTravel.Domain.Entities.Tours.TourPriceTier, Features.Tours.GetTourById.DTOs.PriceDto>()
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Price.Amount))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Price.Currency));
        }
    }
}
