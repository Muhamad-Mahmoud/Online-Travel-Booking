using OnlineTravel.Application.Common.Exceptions;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Application.Specifications;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Flights;

namespace OnlineTravel.Application.Features.Bookings.Pricing
{
    public class FlightBookingPricing
    {
        private readonly IUnitOfWork _unitOfWork;

        public FlightBookingPricing(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Money> CalculateAsync(Guid flightId, DateRange stayRange)
        {
            var flight = await _unitOfWork.Repository<Flight>().GetByIdAsync(flightId)
                         ?? throw new NotFoundException(nameof(Flight), flightId);

            // Validate Flight 
            if (DateOnly.FromDateTime(flight.Schedule.Start) != stayRange.Start)
                throw new BadRequestException($"Flight {flight.FlightNumber.Value} date {flight.Schedule.Start:yyyy-MM-dd} does not match selected date {stayRange.Start:yyyy-MM-dd}.");

            // Validate Availability (Check if any seats are available)
            var seatSpec = new BaseSpecification<FlightSeat>(s => s.FlightId == flightId && s.IsAvailable);
            var availableSeatsCount = await _unitOfWork.Repository<FlightSeat>().GetCountAsync(seatSpec);

            if (availableSeatsCount <= 0)
                throw new BadRequestException($"No available seats for Flight {flight.FlightNumber.Value}.");

            var spec = new FlightFareByFlightSpec(flightId);
            var allFares = await _unitOfWork.Repository<FlightFare>().GetAllWithSpecAsync(spec);
            var fare = allFares.FirstOrDefault();

            if (fare == null)
                throw new BadRequestException($"No fares found for Flight {flight.FlightNumber.Value}.");

            return fare.BasePrice;
        }
    }
}
