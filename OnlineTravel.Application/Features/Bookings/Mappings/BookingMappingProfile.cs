using AutoMapper;
using OnlineTravel.Application.Features.Bookings.DTOs;
using OnlineTravel.Domain.Entities.Bookings;

namespace OnlineTravel.Application.Features.Bookings.Mappings;

public class BookingMappingProfile : Profile
{
    public BookingMappingProfile()
    {
        CreateMap<BookingEntity, BookingResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.IsExpired, opt => opt.MapFrom(src => src.IsExpired(DateTime.UtcNow)))

            .ForMember(dest => dest.BookingReference, opt => opt.MapFrom(src => src.BookingReference.Value))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalPrice.Amount))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.TotalPrice.Currency))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Details.FirstOrDefault() != null ? src.Details.FirstOrDefault()!.CategoryId : Guid.Empty))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Details.FirstOrDefault() != null ? src.Details.FirstOrDefault()!.Category.Title : string.Empty))
            .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.Details.FirstOrDefault() != null ? src.Details.FirstOrDefault()!.ItemId : Guid.Empty))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus.ToString()))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.Details.FirstOrDefault() != null ? src.Details.FirstOrDefault()!.StayRange.Start : DateTime.MinValue))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.Details.FirstOrDefault() != null ? src.Details.FirstOrDefault()!.StayRange.End : DateTime.MinValue));
    }
}
