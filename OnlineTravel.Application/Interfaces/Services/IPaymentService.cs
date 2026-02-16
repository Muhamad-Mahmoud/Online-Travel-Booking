using OnlineTravel.Domain.Entities.Bookings;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Application.Features.Payments.DTOs;

namespace OnlineTravel.Application.Interfaces.Services;

public interface IPaymentService
{
    Task<Result<PaymentResponse>> CreateCheckoutSessionAsync(BookingEntity booking, CancellationToken ct = default);
    Task<Result> ConfirmBookingPaymentAsync(Guid bookingId, CancellationToken ct = default);
}
