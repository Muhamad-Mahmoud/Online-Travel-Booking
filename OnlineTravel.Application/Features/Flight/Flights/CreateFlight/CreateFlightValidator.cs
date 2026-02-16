using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Flight.Flights.CreateFlight
{
    public class CreateFlightValidator:AbstractValidator<CreateFlightCommand>
    {
        public CreateFlightValidator()
        {
            RuleFor(v => v.FlightNumber).NotEmpty().MaximumLength(10);
            RuleFor(v => v.OriginAirportId).NotEmpty().NotEqual(v => v.DestinationAirportId)
                .WithMessage("Origin and Destination airports cannot be the same.");

            RuleFor(v => v.DepartureTime).GreaterThan(DateTime.Now)
                .WithMessage("Departure time must be in the future.");

            RuleFor(v => v.ArrivalTime).GreaterThan(v => v.DepartureTime)
                .WithMessage("Arrival time must be after departure time.");

            RuleFor(v => v.CarrierId).NotEmpty();
            RuleFor(v => v.CategoryId).NotEmpty();
        }
    }
}
