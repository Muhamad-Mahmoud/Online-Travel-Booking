namespace OnlineTravel.Application.Features.Bookings.Shared.DTOs
{
	public sealed record CancelBookingResponse(
		string BookingReference,
		string Status
	);
}
