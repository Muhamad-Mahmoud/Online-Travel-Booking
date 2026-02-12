using OnlineTravel.Domain.Entities.Tours;

namespace OnlineTravel.Application.Specifications.BookingSpec;

public class TourScheduleByDateRangeSpec : BaseSpecification<TourSchedule>
{
    public TourScheduleByDateRangeSpec(Guid tourId, DateOnly start, DateOnly end)
        : base(s => s.TourId == tourId &&
                    s.DateRange.Start <= start &&
                    s.DateRange.End >= end)
    {
    }
}
