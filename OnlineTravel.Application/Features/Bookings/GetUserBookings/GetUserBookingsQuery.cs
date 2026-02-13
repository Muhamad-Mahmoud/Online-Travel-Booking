using MediatR;
using OnlineTravel.Application.Features.Bookings.DTOs;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Bookings.GetUserBookings;

public sealed record GetUserBookingsQuery(Guid UserId) : IRequest<Result<IReadOnlyList<BookingResponse>>>;
