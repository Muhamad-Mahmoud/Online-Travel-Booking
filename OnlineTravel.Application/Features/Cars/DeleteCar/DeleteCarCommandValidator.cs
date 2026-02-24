using FluentValidation;
using OnlineTravel.Application.Features.Cars.DeleteCar;

namespace OnlineTravel.Application.Features.Cars.DeleteCar
{
    /// <summary>
    /// Validator for DeleteCarCommand.
    /// Validates that the car ID is provided and in a valid format for deletion.
    /// </summary>
    public class DeleteCarCommandValidator : AbstractValidator<DeleteCarCommand>
    {
        public DeleteCarCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Car ID is required");
        }
    }
}
