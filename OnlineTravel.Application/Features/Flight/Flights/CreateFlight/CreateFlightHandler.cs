using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Flights.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Flight.Flights.CreateFlight
{
    public class CreateFlightHandler:IRequestHandler<CreateFlightCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateFlightHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateFlightCommand request, CancellationToken cancellationToken)
        {
            // 1. Create Value Objects
            var flightNumber = new FlightNumber(request.FlightNumber);
            var schedule = new DateTimeRange(request.DepartureTime, request.ArrivalTime);

            // 2. Map Command to Flight Entity
            var flight = new OnlineTravel.Domain.Entities.Flights.Flight
            {
                FlightNumber = flightNumber,
                CarrierId = request.CarrierId,
                OriginAirportId = request.OriginAirportId,
                DestinationAirportId = request.DestinationAirportId,
                Schedule = schedule,
                BaggageRules = request.BaggageRules,
                Refundable = request.Refundable,
                CategoryId = request.CategoryId,
                Status = OnlineTravel.Domain.Enums.FlightStatus.Scheduled
            };

            // 2.5 Set Metadata if provided
            if (!string.IsNullOrWhiteSpace(request.Gate) || !string.IsNullOrWhiteSpace(request.Terminal) || !string.IsNullOrWhiteSpace(request.AircraftType))
            {
                flight.Metadata = new FlightMetadata(
                    request.Gate ?? "",
                    request.Terminal ?? "",
                    "", // Remarks
                    request.AircraftType ?? ""
                );
            }

            // 3. Persist via Unit of Work
            await _unitOfWork.Repository<OnlineTravel.Domain.Entities.Flights.Flight>().AddAsync(flight);
            var affectedRows = await _unitOfWork.Complete();
            if (affectedRows <= 0)
            {
                return Result<Guid>.Failure(Error.InternalServer("Failed to create flight."));
            }

            return Result<Guid>.Success(flight.Id);
        }
    }
}
