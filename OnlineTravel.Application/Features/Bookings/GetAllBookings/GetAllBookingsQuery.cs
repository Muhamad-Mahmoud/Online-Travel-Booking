using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Application.Features.Bookings.Shared.DTOs;

namespace OnlineTravel.Application.Features.Bookings.GetAllBookings;

public sealed record GetAllBookingsQuery(int PageIndex, int PageSize, string? SearchTerm = null, string? Status = null) : IRequest<OnlineTravel.Domain.ErrorHandling.Result<PagedResult<AdminBookingResponse>>>;
