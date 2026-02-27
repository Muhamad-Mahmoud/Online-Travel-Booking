namespace OnlineTravel.Mvc.Models;

using OnlineTravel.Application.Common;
using OnlineTravel.Application.Features.Bookings.Shared.DTOs;

public class BookingsIndexViewModel
{
    public PagedResult<AdminBookingResponse> Bookings { get; set; } = new(new List<AdminBookingResponse>(), 0, 1, 10);
    public BookingStatsDto Stats { get; set; } = new();
}
