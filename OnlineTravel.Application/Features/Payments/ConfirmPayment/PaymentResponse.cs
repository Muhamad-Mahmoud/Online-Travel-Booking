namespace OnlineTravel.Application.Features.Payments.ConfirmPayment
{
	public class PaymentResponse
	{
		public string? StripeSessionId { get; set; }
		public string? PaymentIntentId { get; set; }
		public string? PaymentUrl { get; set; }
	}
}
