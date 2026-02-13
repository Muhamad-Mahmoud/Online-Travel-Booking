using OnlineTravel.Domain.Entities._Shared.ValueObjects;

namespace OnlineTravel.Application.Features.Bookings.Strategies;

public record BookingProcessResult(
    Money TotalPrice,
    string? ReservationReference = null
);
