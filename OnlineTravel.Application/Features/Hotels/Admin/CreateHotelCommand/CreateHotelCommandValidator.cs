using FluentValidation;
using OnlineTravel.Application.Interfaces.Persistence;

namespace OnlineTravel.Application.Features.Hotels.Admin.CreateHotelCommand
{
    public class CreateHotelCommandValidator : AbstractValidator<CreateHotelCommand>
    {
        private readonly IHotelRepository _hotelRepository;

        public CreateHotelCommandValidator(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(200).WithMessage("Name must not exceed 200 characters");

            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug is required")
                .MaximumLength(200).WithMessage("Slug must not exceed 200 characters")
                .MustAsync(BeUniqueSlug).WithMessage("Slug already exists");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required")
                .MaximumLength(100);

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Country is required")
                .MaximumLength(100);

            RuleFor(x => x.CancellationPolicy)
                .NotEmpty().WithMessage("Cancellation policy is required")
                .MaximumLength(1000);

            RuleFor(x => x.CheckInTimeEnd)
                .GreaterThan(x => x.CheckInTimeStart).WithMessage("Check-in end time must be after start time");
            RuleFor(x => x.CheckOutTimeEnd)
                .GreaterThan(x => x.CheckOutTimeStart).WithMessage("Check-out end time must be after start time");
        }

        private async Task<bool> BeUniqueSlug(string slug, CancellationToken cancellationToken)
        {
            return !await _hotelRepository.SlugExistsAsync(slug);
        }
    }
}
