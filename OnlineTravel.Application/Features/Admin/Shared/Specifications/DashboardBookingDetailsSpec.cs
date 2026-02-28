using OnlineTravel.Application.Common.Specifications;
using OnlineTravel.Domain.Entities.Bookings;

namespace OnlineTravel.Application.Features.Admin.Shared.Specifications
{
	public class DashboardBookingDetailsSpec : BaseSpecification<BookingDetail>
	{
		public DashboardBookingDetailsSpec()
		{
			AddIncludes(d => d.Category);
		}
	}
}
