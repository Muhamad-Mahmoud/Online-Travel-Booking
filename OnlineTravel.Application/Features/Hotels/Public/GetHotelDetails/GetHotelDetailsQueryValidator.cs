using FluentValidation;

namespace OnlineTravel.Application.Features.Hotels.Public.GetHotelDetails
{
	public class GetHotelDetailsQueryValidator : AbstractValidator<GetHotelDetailsQuery>
	{
		public GetHotelDetailsQueryValidator()
		{
			RuleFor(x => x.Id)
				.NotEmpty().WithMessage("Hotel ID is required");
		}
	}

}
