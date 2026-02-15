using OnlineTravel.Domain.Entities.Bookings;
using OnlineTravel.Domain.Enums;
using OnlineTravel.Application.Specifications;

namespace OnlineTravel.Application.Features.Bookings.Specifications.Availability
{
    /// <summary>
    /// Filters BookingDetails for a specific item that overlap
    /// with the requested date range and are still active.
    /// </summary>

    public class OverlappingBookingDetailsSpec : BaseSpecification<BookingDetail>
    {
        public OverlappingBookingDetailsSpec(Guid itemId, DateTime start, DateTime end, DateTime now)
            : base(bd => bd.ItemId == itemId &&
                         bd.StayRange.Start < end &&
                         bd.StayRange.End > start && // Check for any overlap
                         bd.Booking.Status != BookingStatus.Cancelled &&
                         bd.Booking.Status != BookingStatus.Refunded &&
                         // Expired pending bookings are not considered overlapping
                         !(bd.Booking.Status == BookingStatus.PendingPayment && now > bd.Booking.ExpiresAt))
        {
        }
    }
}
