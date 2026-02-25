using OnlineTravel.Application.Common;
using OnlineTravel.Application.Features.Bookings.Shared.DTOs;

namespace OnlineTravelBookingTeamB.Models;

public class AdminBookingsViewModel
{
    public PagedResult<AdminBookingResponse> Bookings { get; set; } = null!;
    public BookingStatsDto Stats { get; set; } = new();
}
