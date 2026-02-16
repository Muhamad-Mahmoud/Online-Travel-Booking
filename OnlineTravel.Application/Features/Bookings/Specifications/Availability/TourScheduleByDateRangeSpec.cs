using OnlineTravel.Domain.Entities.Tours;
using OnlineTravel.Application.Specifications;

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
