using FluentValidation;

namespace OnlineTravel.Application.Features.Bookings.CancelBooking
{
	public class CancelBookingCommandValidator : AbstractValidator<CancelBookingCommand>
	{
		public CancelBookingCommandValidator()
		{
			RuleFor(x => x.BookingId).NotEmpty();
		}
	}
}
