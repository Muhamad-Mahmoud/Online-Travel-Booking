using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Application.Features.Bookings.Shared;

namespace OnlineTravel.Application.Features.Bookings.GetUserBookings;

public sealed record GetUserBookingsQuery(
	Guid UserId,
	int PageIndex = 1,
	int PageSize = 10
) : IRequest<Result<OnlineTravel.Application.Common.PagedResult<AdminBookingResponse>>>;

