namespace OnlineTravel.Application.Features.Payments.DTOs;

public sealed record PaymentResponse
{
    public string? PaymentUrl { get; init; }
    public string? StripeSessionId { get; init; }
    public string? PaymentIntentId { get; init; }
}
