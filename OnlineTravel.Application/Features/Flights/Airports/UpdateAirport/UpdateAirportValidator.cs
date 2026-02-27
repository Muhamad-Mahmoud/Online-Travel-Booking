using FluentValidation;

namespace OnlineTravel.Application.Features.Flights.Airport.UpdateAirport
{
	public class UpdateAirportValidator : AbstractValidator<UpdateAirportCommand>
	{
		public UpdateAirportValidator()
		{
			RuleFor(v => v.Id).NotEmpty().WithMessage("Airport ID is required for update.");
			RuleFor(v => v.Name).NotEmpty().MaximumLength(200);
			RuleFor(v => v.Code).NotEmpty().Length(3);
			RuleFor(v => v.City).NotEmpty();
			RuleFor(v => v.Country).NotEmpty();
		}
	}
}
