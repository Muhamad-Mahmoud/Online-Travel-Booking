namespace OnlineTravel.Application.Features.Bookings.Shared.DTOs;

public sealed record BookingResponse
{
    public string BookingReference { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string PaymentStatus { get; init; } = null!;
    public DateTime BookingDate { get; init; }
    public decimal TotalAmount { get; init; }
    public string Currency { get; init; } = string.Empty;

    // Flattened Details
    public string Type { get; init; } = string.Empty;
    public string ItemName { get; init; } = string.Empty;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
}
