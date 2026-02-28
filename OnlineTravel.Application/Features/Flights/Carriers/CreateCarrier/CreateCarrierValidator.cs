using FluentValidation;

namespace OnlineTravel.Application.Features.Flights.Carrier.CreateCarrier
{
	public class CreateCarrierValidator : AbstractValidator<CreateCarrierCommand>
	{
		public CreateCarrierValidator()
		{
			RuleFor(c => c.Name).NotEmpty().MaximumLength(100);
			RuleFor(c => c.Code).NotEmpty().Length(2, 3); // Carrier codes are usually 2 or 3 chars
		}

	}
}
