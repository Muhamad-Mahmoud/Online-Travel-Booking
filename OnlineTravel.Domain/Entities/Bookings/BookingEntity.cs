using OnlineTravel.Domain.Entities._Base;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Bookings.ValueObjects;
using OnlineTravel.Domain.Entities.Users;
using OnlineTravel.Domain.Enums;

namespace OnlineTravel.Domain.Entities.Bookings;

public class BookingEntity : BaseEntity
{
    public BookingReference BookingReference { get; private set; } = null!;

    public Guid UserId { get; private set; }


    public BookingStatus Status { get; private set; } = BookingStatus.Pending;

    public Money TotalPrice { get; private set; } = null!;


    public PaymentStatus PaymentStatus { get; private set; } = PaymentStatus.Pending;

    public DateTime BookingDate { get; private set; } = DateTime.UtcNow;

    public virtual AppUser User { get; private set; } = null!;

    public virtual ICollection<BookingDetail> Details { get; private set; } = new List<BookingDetail>();

    protected BookingEntity() { } // For EF

    public static BookingEntity Create(Guid userId, BookingReference reference, Money totalPrice)
    {
        return new BookingEntity
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            BookingReference = reference,
            TotalPrice = totalPrice,
            Status = BookingStatus.Pending,
            PaymentStatus = PaymentStatus.Pending,
            BookingDate = DateTime.UtcNow
        };
    }

    public void AddDetail(BookingDetail detail)
    {
        Details.Add(detail);
    }
}