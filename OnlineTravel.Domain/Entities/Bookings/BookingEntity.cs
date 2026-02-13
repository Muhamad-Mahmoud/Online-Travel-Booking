using OnlineTravel.Domain.Entities._Base;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Bookings.ValueObjects;
using OnlineTravel.Domain.Entities.Users;
using OnlineTravel.Domain.Enums;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Domain.Entities.Bookings;

public class BookingEntity : BaseEntity
{
    public BookingReference BookingReference { get; private set; } = null!;

    public Guid UserId { get; private set; }


    public BookingStatus Status { get; private set; } = BookingStatus.Pending;

    public Money TotalPrice { get; private set; } = null!;


    public PaymentStatus PaymentStatus { get; private set; } = PaymentStatus.Pending;

    public DateTime BookingDate { get; private set; } = DateTime.UtcNow;
    
    public DateTime ExpiresAt { get; private set; }

    public bool IsExpired(DateTime now) => Status == BookingStatus.Pending && now > ExpiresAt;

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
            BookingDate = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddMinutes(15)
        };
    }

    public void AddDetail(BookingDetail detail)
    {
        Details.Add(detail);
    }

    public void Cancel()
    {
        if (Status == BookingStatus.Cancelled)
        {
            return;
        }

        if (Status == BookingStatus.Confirmed)
        {
            throw new DomainException("Cannot cancel a confirmed booking.");
        }

        Status = BookingStatus.Cancelled;
    }

    public void ConfirmPayment()
    {
        if (Status != BookingStatus.Pending)
        {
            throw new DomainException("Only pending bookings can be confirmed.");
        }

        if (IsExpired(DateTime.UtcNow))
        {
            Status = BookingStatus.Cancelled;
            throw new DomainException("Booking has expired and cannot be confirmed.");
        }

        Status = BookingStatus.Confirmed;
        PaymentStatus = PaymentStatus.Paid;
    }
}