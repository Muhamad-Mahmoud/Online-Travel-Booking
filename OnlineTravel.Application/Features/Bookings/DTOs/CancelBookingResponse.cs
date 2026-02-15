namespace OnlineTravel.Application.Features.Bookings.DTOs
{
	public sealed record CancelBookingResponse(
		string BookingReference,
		string Status
	);
}
