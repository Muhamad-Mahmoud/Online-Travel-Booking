using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Flights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Flight.Airport.GetAirportById
{
    public class GetAirportByIdHandler: IRequestHandler<GetAirportByIdQuery, GetAirportByIdDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAirportByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetAirportByIdDto> Handle(GetAirportByIdQuery request, CancellationToken cancellationToken)
        {
            // Fetch the entity from repository using the Id
            var airport = await _unitOfWork.Repository< OnlineTravel.Domain.Entities.Flights.Airport> ().GetByIdAsync(request.Id);

            //  Check if airport exists
            if (airport == null)
            {
                return null; // Or throw a NotFoundException
            }

            //  Map Entity to Dto
            return new GetAirportByIdDto
            {
                Id = airport.Id,
                Name = airport.Name,
                Code = airport.Code.Value,
                FullAddress = $"{airport.Address.Street}, {airport.Address.City}, {airport.Address.Country}",
                Facilities = airport.Facilities
            };
        }
    }
}
