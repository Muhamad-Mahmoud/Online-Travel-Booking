using OnlineTravel.Domain.Entities.Bookings;
using OnlineTravel.Domain.Enums;

namespace OnlineTravel.Application.Specifications.BookingSpec
{
	/// <summary>
	/// Filters BookingDetails for a specific item that overlap
	/// with the requested date range and are still active.
	/// </summary>

	public class OverlappingBookingDetailsSpec : BaseSpecification<BookingDetail>
    {
        public OverlappingBookingDetailsSpec (Guid itemId, DateOnly start, DateOnly end)
            : base(bd => bd.ItemId == itemId &&
                         bd.StayRange.Start < end.ToDateTime(TimeOnly.MaxValue) &&  
						 bd.StayRange.End > start.ToDateTime(TimeOnly.MinValue) && // Check for any overlap
						 bd.Booking.Status != BookingStatus.Cancelled &&
                         bd.Booking.Status != BookingStatus.Refunded)
        {
            AddIncludes(bd => bd.Booking);
        }
    }
}
