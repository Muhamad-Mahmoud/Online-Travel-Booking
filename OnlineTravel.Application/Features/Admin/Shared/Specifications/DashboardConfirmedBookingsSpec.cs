using OnlineTravel.Application.Common.Specifications;
using OnlineTravel.Domain.Entities.Bookings;
using OnlineTravel.Domain.Enums;

namespace OnlineTravel.Application.Features.Admin.Shared.Specifications
{
	public class DashboardConfirmedBookingsSpec : BaseSpecification<BookingEntity>
	{
		public DashboardConfirmedBookingsSpec()
		{
			Criteria = b => b.Status == BookingStatus.Confirmed;
		}

		public DashboardConfirmedBookingsSpec(DateTime fromDate)
		{
			Criteria = b => b.Status == BookingStatus.Confirmed && b.BookingDate >= fromDate;
		}
	}
}
