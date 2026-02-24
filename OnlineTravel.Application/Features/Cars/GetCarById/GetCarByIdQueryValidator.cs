using FluentValidation;
using OnlineTravel.Application.Features.Cars.GetCarById;

namespace OnlineTravel.Application.Features.Cars.GetCarById
{
    public class GetCarByIdQueryValidator : AbstractValidator<GetCarByIdQuery>
    {
        public GetCarByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Car ID is required");
        }
    }
}
