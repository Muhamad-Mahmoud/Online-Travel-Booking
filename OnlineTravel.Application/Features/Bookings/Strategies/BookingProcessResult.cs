using OnlineTravel.Domain.Entities._Shared.ValueObjects;

namespace OnlineTravel.Application.Features.Bookings.Strategies;

public record BookingProcessResult(
    Money TotalPrice,
    string ItemName,
    DateTimeRange BookedRange,
    string? ReservationReference = null
);
