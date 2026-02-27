using OnlineTravel.Application.Common.Specifications;
using OnlineTravel.Domain.Entities.Tours;

namespace OnlineTravel.Application.Features.Admin.Shared.Specifications
{
	public class DashboardActiveTourSchedulesSpec : BaseSpecification<TourSchedule>
	{
		public DashboardActiveTourSchedulesSpec(DateOnly activeFrom)
		{
			Criteria = ts => ts.DateRange.Start >= activeFrom;
		}
	}
}
