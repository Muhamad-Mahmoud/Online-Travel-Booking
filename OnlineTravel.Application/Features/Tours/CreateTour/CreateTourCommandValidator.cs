using FluentValidation;

namespace OnlineTravel.Application.Features.Tours.CreateTour
{
	public class CreateTourCommandValidator : AbstractValidator<CreateTourCommand>
	{
		public CreateTourCommandValidator()
		{
			RuleFor(p => p.Title)
				.NotEmpty().WithMessage("{PropertyName} is required.")
				.NotNull()
				.MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");

			RuleFor(p => p.Description)
				.NotEmpty().WithMessage("{PropertyName} is required.");

			RuleFor(p => p.CategoryId)
				.NotEmpty().WithMessage("{PropertyName} is required.");

			RuleFor(p => p.DurationDays)
				.GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");

			RuleFor(p => p.StandardPrice)
				.GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");

			RuleFor(p => p.MainImageUrl)
				.Must(uri => Uri.TryCreate(uri, UriKind.RelativeOrAbsolute, out _))
				.When(x => !string.IsNullOrEmpty(x.MainImageUrl))
				.WithMessage("{PropertyName} must be a valid URL or path.");
		}
	}
}
