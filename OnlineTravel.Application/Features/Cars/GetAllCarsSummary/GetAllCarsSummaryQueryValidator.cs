using FluentValidation;

namespace OnlineTravel.Application.Features.Cars.GetAllCarsSummary
{
	/// <summary>
	/// Validator for GetAllCarsSummaryQuery.
	/// Validates pagination parameters and filter values for summary listing.
	/// </summary>
	public class GetAllCarsSummaryQueryValidator : AbstractValidator<GetAllCarsSummaryQuery>
	{
		public GetAllCarsSummaryQueryValidator()
		{
			RuleFor(x => x.PageIndex)
				.GreaterThan(0)
				.WithMessage("Page index must be greater than 0");

			RuleFor(x => x.PageSize)
				.GreaterThan(0)
				.WithMessage("Page size must be greater than 0")
				.LessThanOrEqualTo(100)
				.WithMessage("Page size cannot exceed 100");

			RuleFor(x => x.BrandId)
				.Empty()
				.When(x => x.BrandId == Guid.Empty)
				.WithMessage("Brand ID must be valid if provided");

			RuleFor(x => x.CategoryId)
				.Empty()
				.When(x => x.CategoryId == Guid.Empty)
				.WithMessage("Category ID must be valid if provided");
		}
	}
}
