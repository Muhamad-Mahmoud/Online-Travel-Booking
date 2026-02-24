using System.Text.Json.Serialization;

namespace OnlineTravel.Application.Features.Bookings.Shared.DTOs;

public sealed record CreateBookingResponse
{
    public BookingResponse Booking { get; init; } = null!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? PaymentUrl { get; init; }
}
