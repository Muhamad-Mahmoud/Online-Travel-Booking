using OnlineTravel.Domain.Entities.Bookings;

namespace OnlineTravel.Application.Specifications.BookingSpec
{
    public class BookingDetailsByItemSpec : BaseSpecification<BookingDetail>
    {
        public BookingDetailsByItemSpec(Guid itemId, DateOnly start, DateOnly end)
            : base(bd => bd.ItemId == itemId && bd.StayRange.Start < end && bd.StayRange.End > start)
        {
        }
    }
}
