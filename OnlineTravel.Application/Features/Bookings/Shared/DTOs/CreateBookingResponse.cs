

namespace OnlineTravel.Application.Features.Bookings.Shared.DTOs;

public sealed record CreateBookingResponse
{
	public BookingResponse Booking { get; init; } = null!;
}
