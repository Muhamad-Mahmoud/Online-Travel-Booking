using MediatR;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Application.Features.Bookings.DTOs;

namespace OnlineTravel.Application.Features.Bookings.CancelBooking;

public sealed record CancelBookingCommand(
    Guid BookingId,
    Guid UserId
    ) : IRequest<Result<CancelBookingResponse>>;