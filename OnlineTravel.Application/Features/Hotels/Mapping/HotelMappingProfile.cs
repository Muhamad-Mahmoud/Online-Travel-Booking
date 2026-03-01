using Mapster;
using OnlineTravel.Application.Features.Hotels.Shared;
using OnlineTravel.Application.Features.Hotels.Public.SearchHotels;
using OnlineTravel.Application.Features.Reviews.Shared;
using OnlineTravel.Domain.Entities.Hotels;
using OnlineTravel.Domain.Entities.Reviews;

namespace OnlineTravel.Application.Features.Hotels.Mapping;

public class HotelMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<Hotel, HotelSearchResponse>()
			.Map(d => d.Latitude, s => s.Address != null && s.Address.Coordinates != null ? s.Address.Coordinates.Y : 0)
			.Map(d => d.Longitude, s => s.Address != null && s.Address.Coordinates != null ? s.Address.Coordinates.X : 0)
			.Map(d => d.MinPrice, s => s.Rooms.Any() ? s.Rooms.Min(r => r.BasePricePerNight.Amount) : 0m)
			.Map(d => d.TotalRooms, s => s.Rooms.Count)
			.Map(d => d.City, s => s.Address != null ? s.Address.City ?? "" : "")
			.Map(d => d.Country, s => s.Address != null ? s.Address.Country ?? "" : "")
			.Map(d => d.MainImage, s => s.MainImageUrl ?? "")
			.Map(d => d.Rating, s => s.Rating != null ? s.Rating.Value : 0m);

		config.NewConfig<Hotel, HotelDetailsResponse>()
			.Map(d => d.Latitude, s => s.Address != null && s.Address.Coordinates != null ? s.Address.Coordinates.Y : 0)
			.Map(d => d.Longitude, s => s.Address != null && s.Address.Coordinates != null ? s.Address.Coordinates.X : 0)
			.Map(d => d.ReviewCount, s => s.Reviews.Count)
			.Map(d => d.MainImage, s => s.MainImageUrl ?? "")
			.Map(d => d.Rating, s => s.Rating != null ? s.Rating.Value : 0m)
			.Map(d => d.Street, s => s.Address != null ? s.Address.Street ?? "" : "")
			.Map(d => d.City, s => s.Address != null ? s.Address.City ?? "" : "")
			.Map(d => d.State, s => s.Address != null ? s.Address.State ?? "" : "")
			.Map(d => d.Country, s => s.Address != null ? s.Address.Country ?? "" : "")
			.Map(d => d.PostalCode, s => s.Address != null ? s.Address.PostalCode ?? "" : "")
			.Map(d => d.ContactPhone, s => s.ContactInfo != null && s.ContactInfo.Phone != null ? s.ContactInfo.Phone.Value : "")
			.Map(d => d.ContactEmail, s => s.ContactInfo != null && s.ContactInfo.Email != null ? s.ContactInfo.Email.Value : "")
			.Map(d => d.Website, s => s.ContactInfo != null && s.ContactInfo.Website != null ? s.ContactInfo.Website.Value : "")
			.Map(d => d.CheckInTime, s => s.CheckInTime.ToString())
			.Map(d => d.CheckOutTime, s => s.CheckOutTime.ToString())
			.Map(d => d.Rooms, s => s.Rooms)
			.Ignore(d => d.Gallery);

		config.NewConfig<Review, OnlineTravel.Application.Features.Hotels.Shared.ReviewResponse>();

		config.NewConfig<Room, RoomResponse>()
			.Map(d => d.BasePricePerNight, s => s.BasePricePerNight.Amount)
			.Map(d => d.Photos, s => s.Photos.Select(p => p.Value).ToList());
	}
}
