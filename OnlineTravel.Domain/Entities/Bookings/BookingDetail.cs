using OnlineTravel.Domain.Entities._Base;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Core;

namespace OnlineTravel.Domain.Entities.Bookings;

public class BookingDetail : BaseEntity
{
	public Guid BookingId { get; private set; }
	public Guid CategoryId { get; private set; }
	public Guid ItemId { get; private set; }
	public string ItemName { get; private set; } = string.Empty;
	public DateTimeRange StayRange { get; private set; } = null!;

	public virtual BookingEntity Booking { get; private set; } = null!;
	public virtual Category Category { get; private set; } = null!;

	protected BookingDetail() { } // For EF

	public static BookingDetail Create(Guid categoryId, Guid itemId, string itemName, DateTimeRange stayRange)
	{
		return new BookingDetail
		{
			Id = Guid.NewGuid(),
			CategoryId = categoryId,
			ItemId = itemId,
			ItemName = itemName,
			StayRange = stayRange
		};
	}
}




