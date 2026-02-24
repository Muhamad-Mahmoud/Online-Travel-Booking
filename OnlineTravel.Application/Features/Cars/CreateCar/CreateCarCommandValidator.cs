using FluentValidation;
using OnlineTravel.Application.Features.Cars.CreateCar;

namespace OnlineTravel.Application.Features.Cars.CreateCar
{
    /// <summary>
    /// Validator for CreateCarCommand.
    /// Validates all required fields, data types, and business rules for creating a new car.
    /// </summary>
    public class CreateCarCommandValidator : AbstractValidator<CreateCarCommand>
    {
        public CreateCarCommandValidator()
        {
            RuleFor(x => x.Data)
                .NotNull()
                .WithMessage("Car data is required");

            RuleFor(x => x.Data.BrandId)
                .NotEmpty()
                .WithMessage("Brand ID is required");

            RuleFor(x => x.Data.Make)
                .NotEmpty()
                .WithMessage("Make is required")
                .MinimumLength(2)
                .WithMessage("Make must be at least 2 characters")
                .MaximumLength(100)
                .WithMessage("Make must not exceed 100 characters");

            RuleFor(x => x.Data.Model)
                .NotEmpty()
                .WithMessage("Model is required")
                .MinimumLength(1)
                .WithMessage("Model must not be empty")
                .MaximumLength(100)
                .WithMessage("Model must not exceed 100 characters");

            RuleFor(x => x.Data.CarType)
                .IsInEnum()
                .WithMessage("Car type is not valid");

            RuleFor(x => x.Data.SeatsCount)
                .GreaterThan(0)
                .WithMessage("Seats count must be greater than 0")
                .LessThanOrEqualTo(50)
                .WithMessage("Seats count cannot exceed 50");

            RuleFor(x => x.Data.FuelType)
                .IsInEnum()
                .WithMessage("Fuel type is not valid");

            RuleFor(x => x.Data.Transmission)
                .IsInEnum()
                .WithMessage("Transmission type is not valid");

            RuleFor(x => x.Data.CategoryId)
                .NotEmpty()
                .WithMessage("Category ID is required");

            RuleFor(x => x.Data.Location)
                .NotNull()
                .WithMessage("Location is required");

            RuleFor(x => x.Data.CancellationPolicy)
                .MaximumLength(500)
                .WithMessage("Cancellation policy must not exceed 500 characters")
                .When(x => !string.IsNullOrEmpty(x.Data.CancellationPolicy));

            RuleFor(x => x.Data.Features)
                .NotNull()
                .WithMessage("Features list is required");

            RuleFor(x => x.Data.AvailableDates)
                .NotNull()
                .WithMessage("Available dates list is required");

            RuleFor(x => x.Data.Images)
                .NotNull()
                .WithMessage("Images list is required");
        }
    }
}
