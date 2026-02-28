namespace OnlineTravel.Application.Features.Bookings.CancelBooking
{
	public class CancelBookingResponse
	{
		public CancelBookingResponse() { }
		public CancelBookingResponse(string bookingReference, string status)
		{
			BookingReference = bookingReference;
			Status = status;
			Success = true;
			Message = $"Booking {bookingReference} cancelled successfully.";
		}

		public string BookingReference { get; set; } = string.Empty;
		public string Status { get; set; } = string.Empty;
		public bool Success { get; set; }
		public string Message { get; set; } = string.Empty;
	}
}

