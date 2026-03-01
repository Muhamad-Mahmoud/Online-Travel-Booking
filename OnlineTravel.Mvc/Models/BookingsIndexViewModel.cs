namespace OnlineTravel.Mvc.Models;

using OnlineTravel.Application.Common;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Application.Features.Bookings.Shared;


public class BookingsIndexViewModel
{
    public PagedResult<AdminBookingResponse> Bookings { get; set; } = new(new List<AdminBookingResponse>(), 0, 1, 10);
    public BookingStatsResponse Stats { get; set; } = new();
}

