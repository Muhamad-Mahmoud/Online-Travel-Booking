using OnlineTravel.Domain.Enums;

namespace OnlineTravel.Application.Features.Bookings.DTOs;

public sealed class BookingResponse
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

    public Guid CategoryId { get; init; }
    public string CategoryName { get; init; } = string.Empty;
    public Guid ItemId { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
}
