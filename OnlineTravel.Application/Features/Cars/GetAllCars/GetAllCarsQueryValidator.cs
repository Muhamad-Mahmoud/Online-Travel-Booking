using FluentValidation;
using OnlineTravel.Application.Features.Cars.GetAllCars;

namespace OnlineTravel.Application.Features.Cars.GetAllCars
{
    /// <summary>
    /// Validator for GetAllCarsQuery.
    /// Validates pagination parameters and filter values.
    /// </summary>
    public class GetAllCarsQueryValidator : AbstractValidator<GetAllCarsQuery>
    {
        public GetAllCarsQueryValidator()
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
