namespace OnlineTravel.Application.Features.Bookings.DTOs;

public sealed record AdminBookingResponse
{
    public Guid Id { get; init; }
    public string BookingReference { get; init; } = string.Empty;
    public Guid UserId { get; init; }
    public string UserName { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string PaymentStatus { get; init; } = null!;
    public bool IsExpired { get; init; }
    public DateTime BookingDate { get; init; }
    public decimal TotalAmount { get; init; }
    public string Currency { get; init; } = string.Empty;
    public string? StripeSessionId { get; init; }
    public string? PaymentIntentId { get; init; }
    public string Type { get; init; } = string.Empty;
    public string ItemName { get; init; } = string.Empty;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
}
