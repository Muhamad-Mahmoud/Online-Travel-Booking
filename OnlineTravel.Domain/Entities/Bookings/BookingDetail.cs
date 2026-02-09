using OnlineTravel.Domain.Entities._Base;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Core;

namespace OnlineTravel.Domain.Entities.Bookings;

public class BookingDetail : BaseEntity
{
    public Guid BookingId { get; set; }

    public Guid CategoryId { get; set; }

    public Guid ItemId { get; set; }

    public DateRange StayRange { get; set; } = null!;

    public virtual Booking Booking { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;
}




