using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Flights;
using OnlineTravel.Domain.Entities.Flights.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Flight.Airport.UpdateAirport
{
    public class UpdateAirportHandler: IRequestHandler<UpdateAirportCommand, UpdateAirportResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateAirportHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateAirportResponse> Handle(UpdateAirportCommand request, CancellationToken cancellationToken)
        {
            // 1. Fetch existing airport from database
            var airport = await _unitOfWork.Repository<OnlineTravel.Domain.Entities.Flights.Airport> ().GetByIdAsync(request.Id);

            if (airport == null)
            {
                throw new Exception($"Airport with ID {request.Id} not found.");
            }

            // 2. Update the entity with new data (mapping Value Objects)
            airport.Name = request.Name;
            airport.Code = new IataCode(request.Code);
            airport.Address = new Address(request.Street, request.City, request.State, request.Country, request.ZipCode);
            airport.Facilities = request.Facilities;

            // 3. Update in repository and commit
            _unitOfWork.Repository<OnlineTravel.Domain.Entities.Flights.Airport> ().Update(airport);
            var result = await _unitOfWork.Complete();

            return new UpdateAirportResponse
            {
                Id = airport.Id,
                Name = airport.Name,
                IsSuccess = result > 0
            };
        }
    }
}
