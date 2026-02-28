using MediatR;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Bookings.CancelBooking;

public sealed record CancelBookingCommand(
	Guid BookingId,
	Guid UserId
	) : IRequest<Result<CancelBookingResponse>>;
