namespace OnlineTravel.Application.Features.Bookings.Shared.DTOs;

public sealed record AdminBookingResponse
{
    public Guid Id { get; init; }
    public string BookingReference { get; init; } = string.Empty;
    public Guid UserId { get; init; }
    public string UserName { get; init; } = string.Empty;
    public DateTime ExpiresAt { get; init; }
    
    public string Status { get; init; } = string.Empty;

    public bool IsExpired => Status == "PendingPayment" && DateTime.UtcNow > ExpiresAt;

    public string PaymentStatus { get; init; } = null!;

    public DateTime BookingDate { get; init; }
    public DateTime? PaidAt { get; init; }
    public decimal TotalAmount { get; init; }
    public string Currency { get; init; } = string.Empty;
    public string? StripeSessionId { get; init; }
    public string? PaymentIntentId { get; init; }
    public string UserEmail { get; init; } = string.Empty;
    public DateTime UserJoinedAt { get; init; }


    public string Type { get; init; } = string.Empty;
    public string ItemName { get; init; } = string.Empty;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
}
