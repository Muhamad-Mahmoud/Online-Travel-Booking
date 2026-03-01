using Mapster;
using OnlineTravel.Application.Features.Admin.Dashboard;
using OnlineTravel.Application.Features.Bookings.Shared;
using OnlineTravel.Domain.Entities.Bookings;

namespace OnlineTravel.Application.Features.Bookings.Mappings;

public class BookingMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<BookingEntity, BookingResponse>()
			.Map(dest => dest.BookingReference, src => src.BookingReference.Value)
			.Map(dest => dest.BookingDate, src => src.CreatedAt)
			.Map(dest => dest.TotalAmount, src => src.TotalPrice.Amount)
			.Map(dest => dest.Currency, src => src.TotalPrice.Currency)
			.Map(dest => dest.Status, src => src.Status.ToString())
			.Map(dest => dest.PaymentStatus, src => src.PaymentStatus.ToString())
			.Map(dest => dest.Type, src => src.Details.FirstOrDefault() != null ? src.Details.First().Category.Type.ToString() : string.Empty)
			.Map(dest => dest.ItemName, src => src.Details.FirstOrDefault() != null ? src.Details.First().ItemName : string.Empty)
			.Map(dest => dest.StartDate, src => src.Details.FirstOrDefault() != null ? src.Details.First().StayRange.Start : default)
			.Map(dest => dest.EndDate, src => src.Details.FirstOrDefault() != null ? src.Details.First().StayRange.End : default);

		config.NewConfig<BookingEntity, AdminBookingResponse>()
			.Map(dest => dest.BookingReference, src => src.BookingReference.Value)
			.Map(dest => dest.BookingDate, src => src.CreatedAt)
			.Map(dest => dest.PaidAt, src => src.PaidAt)
			.Map(dest => dest.UserName, src => src.User != null ? src.User.Name : "Unknown")
			.Map(dest => dest.UserEmail, src => src.User != null ? src.User.Email : string.Empty)
			.Map(dest => dest.UserJoinedAt, src => src.User != null ? src.User.CreatedAt : default)
			.Map(dest => dest.IsExpired, src => src.IsExpired(DateTime.UtcNow))
			.Map(dest => dest.TotalAmount, src => src.TotalPrice.Amount)
			.Map(dest => dest.Currency, src => src.TotalPrice.Currency)
			.Map(dest => dest.Status, src => src.Status.ToString())
			.Map(dest => dest.PaymentStatus, src => src.PaymentStatus.ToString())
			.Map(dest => dest.StripeSessionId, src => src.StripeSessionId)
			.Map(dest => dest.PaymentIntentId, src => src.PaymentIntentId)
			.Map(dest => dest.Type, src => src.Details.FirstOrDefault() != null ? src.Details.First().Category.Type.ToString() : string.Empty)
			.Map(dest => dest.ItemName, src => src.Details.FirstOrDefault() != null ? src.Details.First().ItemName : string.Empty)
			.Map(dest => dest.StartDate, src => src.Details.FirstOrDefault() != null ? src.Details.First().StayRange.Start : default)
			.Map(dest => dest.EndDate, src => src.Details.FirstOrDefault() != null ? src.Details.First().StayRange.End : default);

		config.NewConfig<BookingDetail, BookingDetailResponse>()
			.Map(dest => dest.Type, src => src.Category.Type.ToString())
			.Map(dest => dest.ItemName, src => src.ItemName)
			.Map(dest => dest.StartDate, src => src.StayRange.Start)
			.Map(dest => dest.EndDate, src => src.StayRange.End);

		config.NewConfig<BookingEntity, RecentBookingDto>()
			.ConstructUsing(src => new RecentBookingDto(
				src.Id,
				src.BookingReference.Value,
				src.User != null ? src.User.Name : null,
				src.Details.Select(d => d.ItemName).FirstOrDefault(),
				src.BookingDate,
				src.TotalPrice.Amount,
				src.TotalPrice.Currency,
				src.Status.ToString()
			));
	}
}
