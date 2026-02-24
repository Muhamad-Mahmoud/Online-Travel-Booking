using MediatR;
using OnlineTravel.Application.Features.Bookings.Shared.DTOs;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Bookings.GetBookingStats;

public sealed record GetBookingStatsQuery : IRequest<Result<BookingStatsDto>>;
