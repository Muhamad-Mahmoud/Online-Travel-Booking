using OnlineTravel.Application.Specifications;
using OnlineTravel.Domain.Entities.Bookings;

namespace OnlineTravel.Application.Features.Admin.Shared.Specifications
{
    public class DashboardRecentBookingsSpec : BaseSpecification<BookingEntity>
    {
        public DashboardRecentBookingsSpec(int count)
        {
            ApplyOrderByDescending(b => b.BookingDate);
            ApplyPagination(0, count);
            
            AddIncludes(b => b.User);
            AddIncludes(b => b.Details);
        }
    }
}
