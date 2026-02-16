using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using OnlineTravel.Application.Features.Flight.CreateAirport;

namespace OnlineTravel.Application.Features.Flight.Airport.CreateAirport
{
    public class CreateAirportValidator: AbstractValidator<CreateAirportCommand>
    {
        public CreateAirportValidator()
        {
            // Airport Name validation
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Airport name is required.")
                .MaximumLength(200).WithMessage("Airport name must not exceed 200 characters.");

            // IATA Code validation
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("IATA code is required.")
                .Length(3).WithMessage("IATA code must be exactly 3 characters.");

            // Address validations
            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required.");

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Country is required.");

            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Street address is required.");

            RuleFor(x => x.ZipCode)
                .NotEmpty().WithMessage("Zip code is required.");

            // Facilities validation
            RuleFor(x => x.Facilities)
                .NotNull().WithMessage("Facilities list cannot be null.");
        }
    }
}
