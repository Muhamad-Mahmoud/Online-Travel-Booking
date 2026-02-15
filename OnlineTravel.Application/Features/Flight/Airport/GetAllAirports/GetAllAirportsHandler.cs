using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Flights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Flight.Airport.GetAllAirports
{
    public class GetAllAirportsHandler: IRequestHandler<GetAllAirportsQuery, List<GetAllAirportsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllAirportsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<GetAllAirportsDto>> Handle(GetAllAirportsQuery request, CancellationToken cancellationToken)
        {
            // 1. Get all airports from the repository
            var airports = await _unitOfWork.Repository<OnlineTravel.Domain.Entities.Flights.Airport> ().GetAllAsync();

            // 2. Map the list of entities to a list of DTOs
            return airports.Select(a => new GetAllAirportsDto
            {
                Id = a.Id,
                Name = a.Name,
                Code = a.Code.Value,
                City = a.Address.City,
                Country = a.Address.Country
            }).ToList();
        }
    }
}
