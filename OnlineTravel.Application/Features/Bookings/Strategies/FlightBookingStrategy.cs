using Microsoft.Extensions.Logging;
using OnlineTravel.Application.Features.Bookings.Specifications.Availability;
using OnlineTravel.Application.Features.Bookings.Specifications.Pricing;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Bookings;
using OnlineTravel.Domain.Entities.Flights;
using OnlineTravel.Domain.Enums;
using OnlineTravel.Domain.ErrorHandling;
using Error = OnlineTravel.Domain.ErrorHandling.Error;

namespace OnlineTravel.Application.Features.Bookings.Strategies;

public class FlightBookingStrategy : IBookingStrategy
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ILogger<FlightBookingStrategy> _logger;

	public FlightBookingStrategy(IUnitOfWork unitOfWork, ILogger<FlightBookingStrategy> logger)
	{
		_unitOfWork = unitOfWork;
		_logger = logger;
	}

	public CategoryType Type => CategoryType.Flight;

	public async Task<Result<BookingProcessResult>> ProcessBookingAsync(Guid itemId, DateTimeRange stayRange, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Checking availability for Flight Seat {SeatId}", itemId);

		// Fetch the Specific Seat first itemId refers to FlightSeatId
		var seat = await _unitOfWork.Repository<FlightSeat>().GetByIdAsync(itemId, cancellationToken);
		if (seat == null)
		{
			_logger.LogWarning("Flight Seat {SeatId} not found", itemId);
			return Result<BookingProcessResult>.Failure(Error.NotFound($"Flight Seat {itemId} was not found."));
		}

		// Fetch the parent Flight
		var flight = await _unitOfWork.Repository<OnlineTravel.Domain.Entities.Flights.Flight>().GetByIdAsync(seat.FlightId, cancellationToken);
		if (flight == null)
		{
			_logger.LogError("Flight not found for Seat {SeatLabel} (Flight ID: {FlightId})", seat.SeatLabel, seat.FlightId);
			return Result<BookingProcessResult>.Failure(Error.NotFound($"Flight for seat {seat.SeatLabel} was not found."));
		}


        // Atomic reservation check inside the entity
        try 
        {
            seat.Reserve();
        }
        catch (DomainException ex)
        {
            _logger.LogWarning("Seat {SeatLabel} reservation failed: {Message}", seat.SeatLabel, ex.Message);
            return Result<BookingProcessResult>.Failure(Error.Validation(ex.Message));
        }

        _unitOfWork.Repository<FlightSeat>().Update(seat);

		//  Calculate Price (Fetching fares for the parent flight)
		var spec = new FlightFareByFlightSpec(seat.FlightId);
		var fares = await _unitOfWork.Repository<FlightFare>().GetAllWithSpecAsync(spec, cancellationToken);

		var fare = fares.FirstOrDefault();

		if (fare == null)
		{
			_logger.LogError("No fare found for Flight {FlightNumber}", flight.FlightNumber.Value);
			return Result<BookingProcessResult>.Failure(Error.Validation($"No fares found for Flight {flight.FlightNumber.Value}."));
		}

		return Result<BookingProcessResult>.Success(new BookingProcessResult(fare.BasePrice, seat.SeatLabel));
	}
}
