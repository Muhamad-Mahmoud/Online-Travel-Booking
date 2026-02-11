using OnlineTravel.Domain.Entities._Base;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Bookings.ValueObjects;
using OnlineTravel.Domain.Entities.Users;
using OnlineTravel.Domain.Enums;

namespace OnlineTravel.Domain.Entities.Bookings;

public class Booking : BaseEntity
{
    public BookingReference BookingReference { get; set; } = null!;

    public Guid UserId { get; set; }


    public BookingStatus Status { get; set; } = BookingStatus.Pending;

    public Money TotalPrice { get; set; } = null!;


    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

    public DateTime BookingDate { get; set; } = DateTime.UtcNow;

    public virtual AppUser User { get; set; } = null!;

    public virtual ICollection<BookingDetail> Details { get; set; } = new List<BookingDetail>();
}




