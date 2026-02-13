using FluentValidation;

namespace OnlineTravel.Application.Features.Bookings.CreateBooking;

public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.ItemId).NotEmpty();
        RuleFor(x => x.StayRange).NotNull();
        RuleFor(x => x.StayRange.Start).GreaterThanOrEqualTo(DateTime.Today);
        RuleFor(x => x.StayRange.End).GreaterThan(cmd => cmd.StayRange.Start);
    }
}
