using MediatR;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Payments.ConfirmPayment;

public sealed record ConfirmPaymentCommand(Guid BookingId, string? PaymentIntentId = null, string? EventId = null) : IRequest<Result>;
