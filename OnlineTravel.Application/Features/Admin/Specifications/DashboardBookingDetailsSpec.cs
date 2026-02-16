using OnlineTravel.Application.Specifications;
using OnlineTravel.Domain.Entities.Bookings;

namespace OnlineTravel.Application.Features.Admin.Dashboard.Specifications
{
    public class DashboardBookingDetailsSpec : BaseSpecification<BookingDetail>
    {
        public DashboardBookingDetailsSpec()
        {
            AddIncludes(d => d.Category);
        }
    }
}
