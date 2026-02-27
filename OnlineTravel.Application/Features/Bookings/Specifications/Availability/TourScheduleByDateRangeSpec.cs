using OnlineTravel.Application.Specifications;
using OnlineTravel.Domain.Entities.Tours;

namespace OnlineTravel.Application.Features.Bookings.Specifications.Availability;

public class TourScheduleByDateRangeSpec : BaseSpecification<TourSchedule>
{
	public TourScheduleByDateRangeSpec(Guid tourId, DateOnly start, DateOnly end)
		: base(s => s.TourId == tourId &&
					s.DateRange.Start <= start &&
					s.DateRange.End >= end)
	{
	}
}
