using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Flights.ValueObjects;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Flight.Flights.UpdateFlight;

public class UpdateFlightHandler : IRequestHandler<UpdateFlightCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateFlightHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(UpdateFlightCommand request, CancellationToken cancellationToken)
    {
        var flight = await _unitOfWork.Repository<OnlineTravel.Domain.Entities.Flights.Flight>()
            .Query()
            .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);

        if (flight == null)
            return Result<bool>.Failure(Error.NotFound("Flight not found."));

        // 1. Update Value Objects
        flight.FlightNumber = new FlightNumber(request.FlightNumber);
        flight.Schedule = new DateTimeRange(request.DepartureTime, request.ArrivalTime);

        // 2. Update Basic Props
        flight.CarrierId = request.CarrierId;
        flight.OriginAirportId = request.OriginAirportId;
        flight.DestinationAirportId = request.DestinationAirportId;
        flight.BaggageRules = request.BaggageRules;
        flight.Refundable = request.Refundable;
        flight.CategoryId = request.CategoryId;
        flight.Status = request.Status;

        // 3. Update Metadata
        flight.Metadata = new FlightMetadata(
            request.Gate ?? "",
            request.Terminal ?? "",
            "", // Remarks
            request.AircraftType ?? ""
        );

        _unitOfWork.Repository<OnlineTravel.Domain.Entities.Flights.Flight>().Update(flight);
        var affectedRows = await _unitOfWork.Complete();
        
        return Result<bool>.Success(affectedRows > 0);
    }
}
