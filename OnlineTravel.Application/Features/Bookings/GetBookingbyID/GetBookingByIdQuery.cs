using MediatR;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Application.Features.Bookings.Shared;

namespace OnlineTravel.Application.Features.Bookings.GetBookingById;

public sealed record GetBookingByIdQuery(Guid BookingId) : IRequest<Result<AdminBookingResponse>>;


