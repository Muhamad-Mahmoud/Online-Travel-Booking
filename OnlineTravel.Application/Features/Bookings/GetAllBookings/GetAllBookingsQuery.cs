using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Application.Features.Bookings.Shared;


namespace OnlineTravel.Application.Features.Bookings.GetAllBookings;

public sealed record GetAllBookingsQuery(int PageIndex, int PageSize, string? SearchTerm = null, string? Status = null) : IRequest<Result<PagedResult<AdminBookingResponse>>>;

