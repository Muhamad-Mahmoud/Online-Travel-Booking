using OnlineTravel.Application.Specifications;
using OnlineTravel.Domain.Entities.Bookings;

namespace OnlineTravel.Application.Features.Bookings.Specifications.Queries

{
    public class GetAllBookingsSpec : BaseSpecification<BookingEntity>
    {
        public GetAllBookingsSpec(int pageIndex, int pageSize)
        {
            AddIncludes(b => b.Details);
            AddIncludes(b => b.Details.Select(d => d.Category));
            AddIncludes(b => b.User);

            ApplyPagination(pageSize * (pageIndex - 1), pageSize);
            AddOrderByDesc(b => b.BookingDate);
        }
    }
}
