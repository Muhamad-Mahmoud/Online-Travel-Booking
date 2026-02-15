using OnlineTravel.Domain.Entities.Bookings;
using OnlineTravel.Domain.Enums;
using OnlineTravel.Application.Specifications;

namespace OnlineTravel.Application.Features.Bookings.Specifications.Availability
{
    /// <summary>
    /// Filters BookingDetails for a specific item (like a FlightSeat) that are still active.
    /// This is used for items that are single-use/event-based and don't depend on a date range overlap.
    /// </summary>
    public class ActiveBookingDetailsByItemSpec : BaseSpecification<BookingDetail>
    {
        public ActiveBookingDetailsByItemSpec(Guid itemId, DateTime now)
            : base(bd => bd.ItemId == itemId &&
                         bd.Booking.Status != BookingStatus.Cancelled &&
                         bd.Booking.Status != BookingStatus.Refunded &&
                         // Expired pending bookings are not considered active
                         !(bd.Booking.Status == BookingStatus.PendingPayment && now > bd.Booking.ExpiresAt))
        {
        }
    }
}
