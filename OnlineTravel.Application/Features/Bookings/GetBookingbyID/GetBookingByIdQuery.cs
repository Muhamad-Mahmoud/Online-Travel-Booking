using MediatR;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Application.Features.Bookings.DTOs;

namespace OnlineTravel.Application.Features.Bookings.GetBookingById;

public sealed record GetBookingByIdQuery(Guid BookingId) : IRequest<Result<AdminBookingResponse>>;
