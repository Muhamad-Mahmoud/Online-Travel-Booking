using AutoMapper;
using OnlineTravel.Application.Features.Hotels.Dtos;
using OnlineTravel.Application.Features.Hotels.Public.SearchHotels;
using OnlineTravel.Domain.Entities.Hotels;
using OnlineTravel.Domain.Entities.Reviews;

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
				.ForMember(d => d.Rooms, opt => opt.MapFrom(s => s.Rooms))
				.ForMember(d => d.Gallery, opt => opt.Ignore());

			CreateMap<Review, ReviewDto>();

			// Room mappings
			CreateMap<Room, RoomDto>()
				.ForMember(d => d.BasePricePerNight, opt => opt.MapFrom(s => s.BasePricePerNight.Amount))
				.ForMember(d => d.Photos, opt => opt.MapFrom(s => s.Photos.Select(p => p.Value).ToList()));

			// Car mappings
			CreateMap<OnlineTravel.Domain.Entities.Cars.CarBrand, OnlineTravel.Application.Features.CarBrands.Shared.DTOs.CarBrandDto>();
			
			CreateMap<OnlineTravel.Domain.Entities.Cars.Car, OnlineTravel.Application.Features.Cars.GetCarById.CarDto>()
				.ForMember(d => d.BrandName, opt => opt.MapFrom(s => s.Brand.Name))
				.ForMember(d => d.CategoryTitle, opt => opt.MapFrom(s => s.Category.Title))
				.ForMember(d => d.Location, opt => opt.MapFrom(s => s.Location));

			CreateMap<OnlineTravel.Domain.Entities._Shared.ValueObjects.DateTimeRange, OnlineTravel.Application.Features.Cars.Shared.DTOs.DateTimeRangeDto>();
			CreateMap<OnlineTravel.Domain.Entities._Shared.ValueObjects.ImageUrl, OnlineTravel.Application.Features.Cars.Shared.DTOs.ImageUrlDto>()
				.ForMember(d => d.Url, opt => opt.MapFrom(s => s.Url));
			CreateMap<NetTopologySuite.Geometries.Point, OnlineTravel.Application.Features.Cars.Shared.DTOs.LocationDto>()
				.ForMember(d => d.Latitude, opt => opt.MapFrom(s => s.Y))
				.ForMember(d => d.Longitude, opt => opt.MapFrom(s => s.X));
		}
	}
}
