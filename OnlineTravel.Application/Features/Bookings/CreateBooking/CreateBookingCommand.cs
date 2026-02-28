using MediatR;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Bookings.CreateBooking;

public sealed record CreateBookingCommand(
	Guid UserId,
	Guid CategoryId,
	Guid ItemId,
	DateTimeRange? StayRange
) : IRequest<Result<CreateBookingResponse>>;
