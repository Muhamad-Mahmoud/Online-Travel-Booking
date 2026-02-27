using FluentValidation;

namespace OnlineTravel.Application.Features.Bookings.CreateBooking;

public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
	public CreateBookingCommandValidator()
	{
		RuleFor(x => x.UserId).NotEmpty();
		RuleFor(x => x.CategoryId).NotEmpty();
		RuleFor(x => x.ItemId).NotEmpty();

		When(x => x.StayRange != null, () =>
		{
			RuleFor(x => x.StayRange!.Start).GreaterThanOrEqualTo(DateTime.Today);
			RuleFor(x => x.StayRange!.End)
				.GreaterThan(cmd => cmd.StayRange!.Start)
				.WithMessage("End date must be after the start date.");
		});
	}
}
