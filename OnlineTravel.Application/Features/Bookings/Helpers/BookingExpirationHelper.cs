using OnlineTravel.Domain.Entities.Bookings;
using OnlineTravel.Domain.Enums;

namespace OnlineTravel.Application.Features.Bookings.Helpers;

public static class BookingExpirationHelper
{
	public static bool MarkExpiredBookings(IEnumerable<BookingEntity> bookings)
	{
		bool hasChanges = false;
		var now = DateTime.UtcNow;

		foreach (var booking in bookings)
		{
			if (booking.Status == BookingStatus.PendingPayment && now > booking.ExpiresAt)
			{
				booking.MarkAsExpired();
				hasChanges = true;
			}
		}

		return hasChanges;
	}
}
