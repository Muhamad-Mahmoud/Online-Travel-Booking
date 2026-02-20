using OnlineTravel.Application.Specifications;
using OnlineTravel.Domain.Entities.Bookings;
using OnlineTravel.Domain.Enums;

namespace OnlineTravel.Application.Features.Admin.Dashboard.Specifications
{
    public class DashboardPendingBookingsSpec : BaseSpecification<BookingEntity>
    {
        public DashboardPendingBookingsSpec(DateTime now)
        {
            Criteria = b => b.Status == BookingStatus.PendingPayment && b.ExpiresAt > now;
        }
    }
}
