using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OnlineTravel.Application.Features.Payments.ConfirmPayment;
using OnlineTravel.Infrastructure.Services.Payments;
using Stripe;
using Stripe.Checkout;

namespace OnlineTravel.Api.Controllers;

[Route("api/v1/payments")]
public class PaymentsController : BaseApiController
{
	private readonly StripeOptions _options;
	private readonly ILogger<PaymentsController> _logger;

	public PaymentsController(IOptions<StripeOptions> options, ILogger<PaymentsController> logger)
	{
		_options = options.Value;
		_logger = logger;
	}

	[HttpPost("webhook")]
	public async Task<IActionResult> StripeWebhook()
	{
		var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
		try
		{
			var stripeEvent = EventUtility.ConstructEvent(
				json,
				Request.Headers["Stripe-Signature"],
				_options.WebhookSecret
			);

			if (stripeEvent.Type == EventTypes.CheckoutSessionCompleted)
			{
				var session = stripeEvent.Data.Object as Session;
				if (session?.Metadata != null && session.Metadata.TryGetValue("BookingId", out var bookingIdStr))
				{
					if (Guid.TryParse(bookingIdStr, out var bookingId))
					{
						var result = await Mediator.Send(new ConfirmPaymentCommand(bookingId, session.PaymentIntentId, stripeEvent.Id));
						if (result.IsFailure)
						{
							_logger.LogWarning("Payment received for Booking {BookingId}, but confirmation failed: {Error}. Service will require manual review.", bookingId, result.Error.Description);
							return Ok(); // Return OK to stop Stripe retries for permanent business failures
						}
					}
				}
			}

			return Ok();
		}
		catch (StripeException e)
		{
			_logger.LogError(e, "Stripe Webhook Error");
			return BadRequest();
		}
	}
}

