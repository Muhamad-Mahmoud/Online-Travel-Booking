using OnlineTravel.Application.Specifications;
using OnlineTravel.Domain.Entities.Bookings;

namespace OnlineTravel.Application.Features.Bookings.Specifications.Queries
{
    public class GetAllBookingsByUserIdSpec : BaseSpecification<BookingEntity>
    {
        public GetAllBookingsByUserIdSpec(Guid UserId) :
            base(b => b.UserId == UserId)
        {
            AddIncludes(b => b.Details);
            AddIncludes(b => b.Details.Select(d => d.Category));
        }
    }
}
