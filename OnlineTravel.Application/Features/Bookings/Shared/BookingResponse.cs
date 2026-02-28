using OnlineTravel.Domain.Enums;

namespace OnlineTravel.Application.Features.Bookings.Shared
{
	public class BookingResponse
	{
		public Guid Id { get; set; }
		public Guid UserId { get; set; }
		public string BookingReference { get; set; } = string.Empty;
		public DateTime BookingDate { get; set; }
		public decimal TotalAmount { get; set; }
		public string Currency { get; set; } = string.Empty;
		public string Status { get; set; } = string.Empty;
		public string PaymentStatus { get; set; } = string.Empty;
		public string? PaymentUrl { get; set; }
		public string? StripeSessionId { get; set; }
		public string Type { get; set; } = string.Empty;
		public string ItemName { get; set; } = string.Empty;
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public List<BookingDetailResponse> Details { get; set; } = new();
	}

	public class BookingDetailResponse
	{
		public Guid Id { get; set; }
		public string Type { get; set; } = string.Empty;
		public string ItemName { get; set; } = string.Empty;
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
	}

	public class AdminBookingResponse : BookingResponse
	{
		public string UserName { get; set; } = string.Empty;
		public string UserEmail { get; set; } = string.Empty;
		public DateTime UserJoinedAt { get; set; }
		public DateTime? PaidAt { get; set; }
		public bool IsExpired { get; set; }
		public string? PaymentIntentId { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}

