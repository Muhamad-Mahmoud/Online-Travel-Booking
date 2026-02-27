using FluentValidation;

namespace OnlineTravel.Application.Features.Hotels.Public.SearchHotels
{
	public class SearchHotelsQueryValidator : AbstractValidator<SearchHotelsQuery>
	{
		public SearchHotelsQueryValidator()
		{
			RuleFor(x => x.PageNumber)
				.GreaterThan(0).WithMessage("Page number must be greater than zero");

			RuleFor(x => x.PageSize)
				.GreaterThan(0).WithMessage("Page size must be greater than zero")
				.LessThanOrEqualTo(100).WithMessage("Page size must not exceed 100");

			RuleFor(x => x.CheckOut)
				.GreaterThan(x => x.CheckIn).WithMessage("Check-out must be after check-in")
				.When(x => x.CheckIn.HasValue && x.CheckOut.HasValue);

			RuleFor(x => x.Stars)
				.InclusiveBetween(1, 5).WithMessage("Stars must be between 1 and 5")
				.When(x => x.Stars.HasValue);
		}
	}

}
