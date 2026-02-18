using OnlineTravel.Application.Specifications;
using OnlineTravel.Domain.Entities.Bookings;

namespace OnlineTravel.Application.Features.Admin.Dashboard.Specifications
{
    public class DashboardNewBookingsSpec : BaseSpecification<BookingEntity>
    {
        public DashboardNewBookingsSpec(DateTime fromDate)
        {
            Criteria = b => b.BookingDate >= fromDate;
        }
    }
}
