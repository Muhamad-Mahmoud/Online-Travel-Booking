using OnlineTravel.Domain.Entities.Bookings;
using OnlineTravel.Domain.Enums;

namespace OnlineTravel.Application.Specifications.BookingSpec
{
    public class BookingDetailsByItemSpec : BaseSpecification<BookingDetail>
    {
        public BookingDetailsByItemSpec(Guid itemId, DateOnly start, DateOnly end)
            : base(bd => bd.ItemId == itemId &&
                         bd.StayRange.Start < end.ToDateTime(TimeOnly.MaxValue) &&
                         bd.StayRange.End > start.ToDateTime(TimeOnly.MinValue) &&
                         bd.Booking.Status != BookingStatus.Cancelled &&
                         bd.Booking.Status != BookingStatus.Refunded)
        {
            AddIncludes(bd => bd.Booking);
        }
    }
}
