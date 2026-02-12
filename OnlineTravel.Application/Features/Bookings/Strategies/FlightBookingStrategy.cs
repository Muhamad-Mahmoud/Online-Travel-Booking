using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Application.Specifications;
using OnlineTravel.Application.Specifications.BookingSpec;
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

    public FlightBookingStrategy(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public CategoryType Type => CategoryType.Flight;

    public async Task<Result<BookingProcessResult>> ProcessBookingAsync(Guid itemId, DateTimeRange stayRange, CancellationToken cancellationToken)
    {
        // Fetch the Specific Seat first itemId refers to FlightSeatId
        var seat = await _unitOfWork.Repository<FlightSeat>().GetByIdAsync(itemId, cancellationToken);
        if (seat == null)
            return Result<BookingProcessResult>.Failure(Error.NotFound($"Flight Seat {itemId} was not found."));

        // Fetch the parent Flight
        var flight = await _unitOfWork.Repository<OnlineTravel.Domain.Entities.Flights.Flight>().GetByIdAsync(seat.FlightId, cancellationToken);
        if (flight == null)
            return Result<BookingProcessResult>.Failure(Error.NotFound($"Flight for seat {seat.SeatLabel} was not found."));


        // Check for seat availability in the Booking table
        var overlappingSpec = new OverlappingBookingDetailsSpec(itemId, DateOnly.FromDateTime(stayRange.Start), DateOnly.FromDateTime(stayRange.End));
        var overlappingBookings = await _unitOfWork.Repository<BookingDetail>().GetAllWithSpecAsync(overlappingSpec, cancellationToken);

        if (overlappingBookings.Any())
            return Result<BookingProcessResult>.Failure(Error.Validation($"Seat {seat.SeatLabel} is already booked for this flight."));

        // Touch the entity to trigger RowVersion check on Save
        _unitOfWork.Repository<FlightSeat>().Update(seat);

        //  Calculate Price (Fetching fares for the parent flight)
        var spec = new FlightFareByFlightSpec(seat.FlightId);
        var fares = await _unitOfWork.Repository<FlightFare>().GetAllWithSpecAsync(spec, cancellationToken);

        var fare = fares.FirstOrDefault();

        if (fare == null)
            return Result<BookingProcessResult>.Failure(Error.Validation($"No fares found for Flight {flight.FlightNumber.Value}."));

        return Result<BookingProcessResult>.Success(new BookingProcessResult(fare.BasePrice, seat.SeatLabel));
    }
}
