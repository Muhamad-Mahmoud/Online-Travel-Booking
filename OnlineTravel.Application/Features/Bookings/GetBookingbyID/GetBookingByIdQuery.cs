using MediatR;
using OnlineTravel.Application.Features.Bookings.Shared;

namespace OnlineTravel.Application.Features.Bookings.GetBookingById;

public sealed record GetBookingByIdQuery(Guid BookingId) : IRequest<OnlineTravel.Application.Common.Result<AdminBookingResponse>>;


