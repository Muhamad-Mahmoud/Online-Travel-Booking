using MediatR;
using OnlineTravel.Application.Features.Bookings.Shared;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Bookings.GetBookingStats;

public sealed record GetBookingStatsQuery : IRequest<Result<BookingStatsResponse>>;
