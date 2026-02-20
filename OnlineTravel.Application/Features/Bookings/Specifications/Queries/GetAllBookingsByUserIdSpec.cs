using OnlineTravel.Application.Specifications;
using OnlineTravel.Domain.Entities.Bookings;

namespace OnlineTravel.Application.Features.Bookings.Specifications.Queries
{
    public class GetAllBookingsByUserIdSpec : BaseSpecification<BookingEntity>
    {
        public GetAllBookingsByUserIdSpec(Guid userId, int pageIndex = 1, int pageSize = 10)
            : base(b => b.UserId == userId)
        {
            AddIncludes(b => b.Details);
            AddIncludes(b => b.Details.Select(d => d.Category));
            AddIncludes(b => b.User);
            AddOrderByDesc(b => b.BookingDate);
            ApplyPagination(pageSize * (pageIndex - 1), pageSize);
        }

        /// <summary>Count-only spec — no includes or paging needed.</summary>
        public GetAllBookingsByUserIdSpec(Guid userId, bool isCountOnly)
            : base(b => b.UserId == userId) { }
    }
}
