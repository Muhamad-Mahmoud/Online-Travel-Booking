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


    public BookingStatus Status { get; private set; } = BookingStatus.PendingPayment;

    public Money TotalPrice { get; private set; } = null!;


    public PaymentStatus PaymentStatus { get; private set; } = PaymentStatus.Pending;

    public string? StripeSessionId { get; private set; }
    public string? PaymentIntentId { get; private set; }

    public DateTime BookingDate { get; private set; } = DateTime.UtcNow;
    
    public DateTime ExpiresAt { get; private set; }

    public bool IsExpired(DateTime now) => Status == BookingStatus.PendingPayment && now > ExpiresAt;

    public virtual AppUser User { get; private set; } = null!;

    public virtual ICollection<BookingDetail> Details { get; private set; } = new List<BookingDetail>();

    protected BookingEntity() { } // For EF

    public static BookingEntity Create(Guid userId, Money totalPrice)
    {
        var reference = new BookingReference($"BK-{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}");
        return new BookingEntity
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            BookingReference = reference,
            TotalPrice = totalPrice,
            Status = BookingStatus.PendingPayment,
            PaymentStatus = PaymentStatus.Pending,
            BookingDate = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddMinutes(15)
        };
    }

    public void UpdateStripeInfo(string sessionId, string paymentIntentId)
    {
        StripeSessionId = sessionId;
        PaymentIntentId = paymentIntentId;
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
        PaymentStatus = PaymentStatus.Cancelled;
    }

    public void MarkAsExpired()
    {
        if (Status == BookingStatus.PendingPayment)
        {
            Status = BookingStatus.Expired;
            PaymentStatus = PaymentStatus.Expired;		}
    }

    public void ConfirmPayment(string? paymentIntentId = null)
    {
        if (Status == BookingStatus.Confirmed)
        {
            return;
        }

        if (Status != BookingStatus.PendingPayment)
        {
            throw new DomainException("Only pending payment bookings can be confirmed.");
        }

        // Allow a 5-minute grace period for Stripe webhook latency
        if (IsExpired(DateTime.UtcNow.AddMinutes(-5)))
        {
            Status = BookingStatus.Expired;
            throw new DomainException("Booking has expired and cannot be confirmed.");
        }

        if (!string.IsNullOrEmpty(paymentIntentId))
        {
            PaymentIntentId = paymentIntentId;
        }

        Status = BookingStatus.Confirmed;
        PaymentStatus = PaymentStatus.Paid;
        PaidAt = DateTime.UtcNow;
    }

    public DateTime? PaidAt { get; private set; }
}