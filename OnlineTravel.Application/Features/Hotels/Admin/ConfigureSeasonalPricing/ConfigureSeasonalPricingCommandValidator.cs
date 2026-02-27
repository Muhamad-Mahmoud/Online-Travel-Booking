using FluentValidation;

namespace OnlineTravel.Application.Features.Hotels.Admin.ConfigureSeasonalPricing
{
	public class ConfigureSeasonalPricingCommandValidator : AbstractValidator<ConfigureSeasonalPricingCommand>
	{
		public ConfigureSeasonalPricingCommandValidator()
		{
			RuleFor(x => x.RoomId)
				.NotEmpty().WithMessage("Room ID is required");

			RuleFor(x => x.StartDate)
				.NotEmpty().WithMessage("Start date is required");

			RuleFor(x => x.EndDate)
				.NotEmpty().WithMessage("End date is required")
				.GreaterThan(x => x.StartDate).WithMessage("End date must be after start date");

			RuleFor(x => x.Price)
				.GreaterThan(0).WithMessage("Price must be greater than zero");
		}
	}
}
