using OnlineTravel.Domain.ErrorHandling;
using MediatR;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;

namespace OnlineTravel.Application.Features.Bookings.Commands.CreateBooking;

public sealed record CreateBookingCommand(
    Guid UserId,
    Guid CategoryId,
    Guid ItemId,
    DateTimeRange StayRange
) : IRequest<Result<Guid>>;