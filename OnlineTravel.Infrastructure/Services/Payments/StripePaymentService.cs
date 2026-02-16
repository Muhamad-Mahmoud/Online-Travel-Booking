using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OnlineTravel.Application.Features.Payments.DTOs;
using OnlineTravel.Application.Interfaces.Services;
using OnlineTravel.Domain.Entities.Bookings;
using OnlineTravel.Domain.ErrorHandling;
using Stripe;
using Stripe.Checkout;

namespace OnlineTravel.Infrastructure.Services.Payments;

public class StripePaymentService : IPaymentService
{
	private readonly StripeOptions _options;
	private readonly ILogger<StripePaymentService> _logger;

	public StripePaymentService(IOptions<StripeOptions> options, ILogger<StripePaymentService> logger)
	{
		_options = options.Value;
		_logger = logger;
		StripeConfiguration.ApiKey = _options.SecretKey;
	}

	public async Task<Result<PaymentResponse>> CreateCheckoutSessionAsync(BookingEntity booking, CancellationToken ct = default)
	{
		_logger.LogInformation("Creating Stripe checkout session for Booking {BookingId}", booking.Id);

		try
		{
			var options = new SessionCreateOptions
			{
				PaymentMethodTypes = new List<string> { "card" },
				LineItems = new List<SessionLineItemOptions>
				{
					new SessionLineItemOptions
					{
						PriceData = new SessionLineItemPriceDataOptions
						{
							UnitAmount = (long)(booking.TotalPrice.Amount * 100), // Stripe uses cents
                            Currency = booking.TotalPrice.Currency.ToLower(),
							ProductData = new SessionLineItemPriceDataProductDataOptions
							{
								Name = $"Travel Booking: {booking.BookingReference.Value}",
								Description = "Payment for your travel reservation",
							},
						},
						Quantity = 1,
					},
				},
				Mode = "payment",
				SuccessUrl = _options.SuccessUrl + "?bookingId=" + booking.Id,
				CancelUrl = _options.CancelUrl + "?bookingId=" + booking.Id,
				Metadata = new Dictionary<string, string>
				{
					{ "BookingId", booking.Id.ToString() }
				}
			};

			var service = new SessionService();
			Session session = await service.CreateAsync(options, cancellationToken: ct);

			return Result<PaymentResponse>.Success(new PaymentResponse
			{
				PaymentUrl = session.Url,
				StripeSessionId = session.Id,
				PaymentIntentId = session.PaymentIntentId
			});
		}
		catch (StripeException ex)
		{
			_logger.LogError(ex, "Stripe error while creating session for Booking {BookingId}", booking.Id);
			return Result<PaymentResponse>.Failure(Error.Validation($"Stripe error: {ex.Message}"));
		}
	}

	public Task<Result> ConfirmBookingPaymentAsync(Guid bookingId, CancellationToken ct = default)
	{
		return Task.FromResult(Result.Success());
	}
}
