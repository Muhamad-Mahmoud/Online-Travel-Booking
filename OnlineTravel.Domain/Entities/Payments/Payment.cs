using OnlineTravel.Domain.Entities._Base;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Enums;

namespace OnlineTravel.Domain.Entities.Payments;

public class Payment : BaseEntity
{
    public Guid BookingId { get; set; }

    public Money Amount { get; set; } = null!;

    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    public string? TransactionId { get; set; }

    public Money? RefundAmount { get; set; }

    public string? RefundTransactionId { get; set; }

    public DateTime? PaidAt { get; set; }

    public DateTime? RefundedAt { get; set; }

    // Navigation Properties

    public virtual Bookings.BookingEntity Booking { get; set; } = null!;
}




