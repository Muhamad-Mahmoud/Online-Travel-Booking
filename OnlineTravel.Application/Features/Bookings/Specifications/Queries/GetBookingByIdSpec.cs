using OnlineTravel.Application.Specifications;
using OnlineTravel.Domain.Entities.Bookings;

namespace OnlineTravel.Application.Features.Bookings.Specifications.Queries

{
    public class GetBookingByIdSpec : BaseSpecification<BookingEntity>
    {
        public GetBookingByIdSpec(Guid BookingId) :
            base(b => b.Id == BookingId)
        {
            AddIncludes(b => b.User);
            AddIncludes(b => b.Details);
            AddIncludes(b => b.Details.Select(d => d.Category));
        }
    }
}
