namespace OnlineTravel.Application.Features.Bookings.Shared.DTOs;

public sealed record BookingDetailResponse
{
	public string Type { get; init; } = string.Empty;
	public string ItemName { get; init; } = string.Empty;
	public DateTime StartDate { get; init; }
	public DateTime EndDate { get; init; }
}
