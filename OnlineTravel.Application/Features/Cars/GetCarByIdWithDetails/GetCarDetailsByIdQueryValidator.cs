using FluentValidation;

namespace OnlineTravel.Application.Features.Cars.GetCarByIdWithDetails
{
	/// <summary>
	/// Validator for GetCarDetailsByIdQuery.
	/// Validates that the car ID is provided in a valid format.
	/// </summary>
	public class GetCarDetailsByIdQueryValidator : AbstractValidator<GetCarDetailsByIdQuery>
	{
		public GetCarDetailsByIdQueryValidator()
		{
			RuleFor(x => x.Id)
				.NotEmpty()
				.WithMessage("Car ID is required");
		}
	}
}
