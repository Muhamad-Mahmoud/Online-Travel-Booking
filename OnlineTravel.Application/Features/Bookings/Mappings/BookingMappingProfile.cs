using AutoMapper;
using OnlineTravel.Application.Features.Admin.Dashboard;
using OnlineTravel.Application.Features.Bookings.Shared.DTOs;
using OnlineTravel.Domain.Entities.Bookings;

namespace OnlineTravel.Application.Features.Bookings.Mappings;

public class BookingMappingProfile : Profile
{
	public BookingMappingProfile()
	{
		CreateMap<BookingEntity, BookingResponse>()
			.ForMember(dest => dest.BookingReference, opt => opt.MapFrom(src => src.BookingReference.Value))
			.ForMember(dest => dest.BookingDate, opt => opt.MapFrom(src => src.CreatedAt))
			.ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalPrice.Amount))
			.ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.TotalPrice.Currency))
			.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
			.ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus.ToString()))
			.ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Details.FirstOrDefault() != null ? src.Details.First().Category.Type.ToString() : string.Empty))
			.ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Details.FirstOrDefault() != null ? src.Details.First().ItemName : string.Empty))
			.ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.Details.FirstOrDefault() != null ? src.Details.First().StayRange.Start : default))
			.ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.Details.FirstOrDefault() != null ? src.Details.First().StayRange.End : default));

		CreateMap<BookingEntity, AdminBookingResponse>()
			.ForMember(dest => dest.BookingReference, opt => opt.MapFrom(src => src.BookingReference.Value))
			.ForMember(dest => dest.BookingDate, opt => opt.MapFrom(src => src.CreatedAt))
			.ForMember(dest => dest.PaidAt, opt => opt.MapFrom(src => src.PaidAt))
			.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.Name : "Unknown"))
			.ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User != null ? src.User.Email : string.Empty))
			.ForMember(dest => dest.UserJoinedAt, opt => opt.MapFrom(src => src.User != null ? src.User.CreatedAt : default))

			.ForMember(dest => dest.IsExpired, opt => opt.MapFrom(src => src.IsExpired(DateTime.UtcNow)))
			.ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalPrice.Amount))
			.ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.TotalPrice.Currency))
			.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
			.ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus.ToString()))
			.ForMember(dest => dest.StripeSessionId, opt => opt.MapFrom(src => src.StripeSessionId))
			.ForMember(dest => dest.PaymentIntentId, opt => opt.MapFrom(src => src.PaymentIntentId))
			.ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Details.FirstOrDefault() != null ? src.Details.First().Category.Type.ToString() : string.Empty))
			.ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Details.FirstOrDefault() != null ? src.Details.First().ItemName : string.Empty))
			.ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.Details.FirstOrDefault() != null ? src.Details.First().StayRange.Start : default))
			.ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.Details.FirstOrDefault() != null ? src.Details.First().StayRange.End : default));

		CreateMap<BookingDetail, BookingDetailResponse>()
			.ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Category.Type.ToString()))
			.ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.ItemName))
			.ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StayRange.Start))
			.ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.StayRange.End));

		CreateMap<BookingEntity, RecentBookingDto>()
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
