using MediatR;
using OnlineTravel.Application.Features.Bookings.DTOs;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Bookings.GetAllBookings;

public sealed record GetAllBookingsQuery(int PageIndex = 1, int PageSize = 10) : IRequest<Result<IReadOnlyList<AdminBookingResponse>>>;
