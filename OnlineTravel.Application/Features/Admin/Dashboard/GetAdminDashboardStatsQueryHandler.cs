using Mapster;
using MediatR;
using OnlineTravel.Application.Features.Admin.Shared.Specifications;
using OnlineTravel.Application.Features.Bookings.Helpers;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Bookings;
using OnlineTravel.Domain.Entities.Tours;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Admin.Dashboard;

public sealed class GetAdminDashboardStatsQueryHandler : IRequestHandler<GetAdminDashboardStatsQuery, Result<AdminDashboardResponse>>
{
	private readonly IUnitOfWork _unitOfWork;

	public GetAdminDashboardStatsQueryHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<AdminDashboardResponse>> Handle(GetAdminDashboardStatsQuery request, CancellationToken cancellationToken)
	{
		var now = DateTime.UtcNow;
		var today = DateOnly.FromDateTime(now);
		var thirtyDaysAgo = now.AddDays(-30);
		var startOfYear = new DateTime(now.Year, 1, 1);

		// Stats Cards
		// Total Revenue: Confirmed bookings
		var confirmedBookingsSpec = new DashboardConfirmedBookingsSpec();
		var confirmedBookings = await _unitOfWork.Repository<BookingEntity>().GetAllWithSpecAsync(confirmedBookingsSpec, cancellationToken);
		var totalRevenue = confirmedBookings.Sum(b => b.TotalPrice.Amount);

		// New Bookings Count
		var newBookingsSpec = new DashboardNewBookingsSpec(thirtyDaysAgo);
		var newBookingsCount = await _unitOfWork.Repository<BookingEntity>().GetCountAsync(newBookingsSpec, cancellationToken);

		// Active Tours Count
		var activeToursSpec = new DashboardActiveTourSchedulesSpec(today);
		var activeSchedules = await _unitOfWork.Repository<TourSchedule>().GetAllWithSpecAsync(activeToursSpec, cancellationToken);
		var activeToursCount = activeSchedules.Select(ts => ts.TourId).Distinct().Count();

		// Pending Requests Count
		var pendingBookingsSpec = new DashboardPendingBookingsSpec(now);
		var pendingRequestsCount = await _unitOfWork.Repository<BookingEntity>().GetCountAsync(pendingBookingsSpec, cancellationToken);

		// Revenue Analytics (Monthly for current year)
		var monthlyRevenueSpec = new DashboardConfirmedBookingsSpec(startOfYear);
		var monthlyRevenueBookings = await _unitOfWork.Repository<BookingEntity>().GetAllWithSpecAsync(monthlyRevenueSpec, cancellationToken);

		var monthlyRevenueData = monthlyRevenueBookings
			.GroupBy(b => b.BookingDate.Month)
			.Select(g => new { Month = g.Key, Revenue = g.Sum(b => b.TotalPrice.Amount) })
			.ToList();

		var months = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
		var revenueAnalytics = months.Select((name, index) => new MonthlyRevenueDto(
			name,
			monthlyRevenueData.FirstOrDefault(m => m.Month == index + 1)?.Revenue ?? 0
		)).ToList();

		// Category Distribution
		var bookingDetailsSpec = new DashboardBookingDetailsSpec();
		var allDetails = await _unitOfWork.Repository<BookingDetail>().GetAllWithSpecAsync(bookingDetailsSpec, cancellationToken);

		var categoryDistributionDtos = allDetails
			.GroupBy(bd => bd.Category.Title)
			.Select(g => new CategoryDistributionDto(g.Key, g.Count()))
			.ToList();

		// Recent Bookings
		var recentBookingsSpec = new DashboardRecentBookingsSpec(5);
		var recentBookingsRaw = await _unitOfWork.Repository<BookingEntity>().GetAllWithSpecAsync(recentBookingsSpec, cancellationToken);

		// Handle lazy expiration
		if (BookingExpirationHelper.MarkExpiredBookings(recentBookingsRaw))
		{
			await _unitOfWork.Complete();
		}

		var recentBookings = recentBookingsRaw.Adapt<List<RecentBookingDto>>();

		return new AdminDashboardResponse
		{
			TotalRevenue = totalRevenue,
			NewBookingsCount = newBookingsCount,
			ActiveToursCount = activeToursCount,
			PendingRequestsCount = pendingRequestsCount,
			RevenueAnalytics = revenueAnalytics,
			CategoryDistribution = categoryDistributionDtos,
			RecentBookings = recentBookings
		};
	}
}
