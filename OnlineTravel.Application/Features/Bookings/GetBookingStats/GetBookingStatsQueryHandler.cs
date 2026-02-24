using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineTravel.Application.Features.Bookings.Shared.DTOs;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Bookings;
using OnlineTravel.Domain.Enums;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Bookings.GetBookingStats;

public sealed class GetBookingStatsQueryHandler : IRequestHandler<GetBookingStatsQuery, Result<BookingStatsDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetBookingStatsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<BookingStatsDto>> Handle(GetBookingStatsQuery request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Repository<BookingEntity>().Query();

        var stats = await query
            .GroupBy(x => 1)
            .Select(g => new BookingStatsDto
            {
                TotalBookings = g.Count(),
                PendingBookings = g.Count(b => b.Status == BookingStatus.PendingPayment),
                TotalRevenue = g.Where(b => b.Status == BookingStatus.Confirmed).Sum(b => b.TotalPrice.Amount),
                CancelledBookings = g.Count(b => b.Status == BookingStatus.Cancelled || b.Status == BookingStatus.Expired)
            })
            .FirstOrDefaultAsync(cancellationToken);

        return Result<BookingStatsDto>.Success(stats ?? new BookingStatsDto());
    }
}
