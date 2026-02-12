using OnlineTravel.Domain.ErrorHandling;
using Error = OnlineTravel.Domain.ErrorHandling.Error;
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

        public async Task<Result<Money>> CalculateAsync(Guid flightId, DateTimeRange stayRange)
        {
            var flight = await _unitOfWork.Repository<OnlineTravel.Domain.Entities.Flights.Flight>().GetByIdAsync(flightId);
            if (flight == null)
                return Result<Money>.Failure(Error.NotFound($"Flight {flightId} was not found."));

            // Validate Flight 
            if (DateOnly.FromDateTime(flight.Schedule.Start) != DateOnly.FromDateTime(stayRange.Start))
                return Result<Money>.Failure(Error.Validation($"Flight {flight.FlightNumber.Value} date {flight.Schedule.Start:yyyy-MM-dd} does not match selected date {stayRange.Start:yyyy-MM-dd}."));

            // Validate Availability (Check if any seats are available)
            var seatSpec = new BaseSpecification<FlightSeat>(s => s.FlightId == flightId && s.IsAvailable);
            var availableSeatsCount = await _unitOfWork.Repository<FlightSeat>().GetCountAsync(seatSpec);

            if (availableSeatsCount <= 0)
                return Result<Money>.Failure(Error.Validation($"No available seats for Flight {flight.FlightNumber.Value}."));

            var spec = new FlightFareByFlightSpec(flightId);
            var allFares = await _unitOfWork.Repository<FlightFare>().GetAllWithSpecAsync(spec);
            var fare = allFares.FirstOrDefault();

            if (fare == null)
                return Result<Money>.Failure(Error.Validation($"No fares found for Flight {flight.FlightNumber.Value}."));

            return Result<Money>.Success(fare.BasePrice);
        }
    }
}
