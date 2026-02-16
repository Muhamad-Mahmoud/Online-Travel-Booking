using AutoMapper;
using OnlineTravel.Application.Features.Hotels.Dtos;
using OnlineTravel.Application.Features.Hotels.Public.SearchHotels;
using OnlineTravel.Domain.Entities.Hotels;
using OnlineTravel.Domain.Entities.Reviews;
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

            // Hotel mappings
            CreateMap<Hotel, HotelSearchDto>()
                .ForMember(d => d.Latitude, opt => opt.MapFrom(s => s.Address != null && s.Address.Coordinates != null ? s.Address.Coordinates.Y : 0))
                .ForMember(d => d.Longitude, opt => opt.MapFrom(s => s.Address != null && s.Address.Coordinates != null ? s.Address.Coordinates.X : 0))
                .ForMember(d => d.MinPrice, opt => opt.MapFrom(s => s.Rooms.Any() ? s.Rooms.Min(r => r.BasePricePerNight.Amount) : 0m))
                .ForMember(d => d.TotalRooms, opt => opt.MapFrom(s => s.Rooms.Count))
                .ForMember(d => d.City, opt => opt.MapFrom(s => s.Address != null ? s.Address.City ?? "" : ""))
                .ForMember(d => d.Country, opt => opt.MapFrom(s => s.Address != null ? s.Address.Country ?? "" : ""))
                .ForMember(d => d.MainImage, opt => opt.MapFrom(s => s.MainImageUrl ?? ""))
                .ForMember(d => d.Rating, opt => opt.MapFrom(s => s.Rating != null ? s.Rating.Value : 0m));

            CreateMap<Hotel, HotelDetailsDto>()
                .ForMember(d => d.Latitude, opt => opt.MapFrom(s => s.Address != null && s.Address.Coordinates != null ? s.Address.Coordinates.Y : 0))
                .ForMember(d => d.Longitude, opt => opt.MapFrom(s => s.Address != null && s.Address.Coordinates != null ? s.Address.Coordinates.X : 0))
                .ForMember(d => d.TotalReviews, opt => opt.MapFrom(s => s.Reviews.Count))
                .ForMember(d => d.MainImage, opt => opt.MapFrom(s => s.MainImageUrl ?? ""))
                .ForMember(d => d.Rating, opt => opt.MapFrom(s => s.Rating != null ? s.Rating.Value : 0m))
                .ForMember(d => d.Street, opt => opt.MapFrom(s => s.Address != null ? s.Address.Street ?? "" : ""))
                .ForMember(d => d.City, opt => opt.MapFrom(s => s.Address != null ? s.Address.City ?? "" : ""))
                .ForMember(d => d.State, opt => opt.MapFrom(s => s.Address != null ? s.Address.State ?? "" : ""))
                .ForMember(d => d.Country, opt => opt.MapFrom(s => s.Address != null ? s.Address.Country ?? "" : ""))
                .ForMember(d => d.PostalCode, opt => opt.MapFrom(s => s.Address != null ? s.Address.PostalCode ?? "" : ""))
                .ForMember(d => d.ContactPhone, opt => opt.MapFrom(s => s.ContactInfo != null && s.ContactInfo.Phone != null ? s.ContactInfo.Phone.Value : ""))
                .ForMember(d => d.ContactEmail, opt => opt.MapFrom(s => s.ContactInfo != null && s.ContactInfo.Email != null ? s.ContactInfo.Email.Value : ""))
                .ForMember(d => d.Website, opt => opt.MapFrom(s => s.ContactInfo != null && s.ContactInfo.Website != null ? s.ContactInfo.Website.Value : ""))
                .ForMember(d => d.CheckInTime, opt => opt.MapFrom(s => s.CheckInTime.Start))
                .ForMember(d => d.CheckOutTime, opt => opt.MapFrom(s => s.CheckOutTime.Start))
                .ForMember(d => d.Gallery, opt => opt.Ignore());

            CreateMap<Review, ReviewDto>();

            // Room mappings
            CreateMap<Room, RoomDto>()
                .ForMember(d => d.BasePricePerNight, opt => opt.MapFrom(s => s.BasePricePerNight.Amount))
                .ForMember(d => d.Photos, opt => opt.MapFrom(s => s.Photos.Select(p => p.Value).ToList()));
        }
    }
}
