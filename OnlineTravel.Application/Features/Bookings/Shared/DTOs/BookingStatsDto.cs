namespace OnlineTravel.Application.Features.Bookings.Shared.DTOs;

public sealed record BookingStatsDto
{
    public int TotalBookings { get; init; }
    public int PendingBookings { get; init; }
    public decimal TotalRevenue { get; init; }
    public int CancelledBookings { get; init; }
}
