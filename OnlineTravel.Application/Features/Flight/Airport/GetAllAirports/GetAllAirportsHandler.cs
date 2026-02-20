using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;

namespace OnlineTravel.Application.Features.Flight.Airport.GetAllAirports
{
    public class GetAllAirportsHandler : IRequestHandler<GetAllAirportsQuery, List<GetAllAirportsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllAirportsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<GetAllAirportsDto>> Handle(GetAllAirportsQuery request, CancellationToken cancellationToken)
        {
            var airports = await _unitOfWork.Repository<OnlineTravel.Domain.Entities.Flights.Airport>().GetAllAsync();

            // Apply pagination in-memory (airports table is small and relatively static master data)
            return airports
                .Skip(request.PageSize * (request.PageIndex - 1))
                .Take(request.PageSize)
                .Select(a => new GetAllAirportsDto
                {
                    Id      = a.Id,
                    Name    = a.Name,
                    Code    = a.Code.Value,
                    City    = a.Address.City,
                    Country = a.Address.Country
                })
                .ToList();
        }
    }
}
