using MediatR;
using OnlineTravel.Application.Features.Flights.Flights.SearchFlights.DTOs;
using OnlineTravel.Application.Features.Flights.Flights.SearchFlights.Queries;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Application.Features.Flights.Specifications;

namespace OnlineTravel.Application.Features.Flights.Flights.SearchFlights.Handlers
{
	public class SearchFlightsHandler : IRequestHandler<SearchFlightsQuery, List<SearchFlightsResponse>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public SearchFlightsHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

		public async Task<List<SearchFlightsResponse>> Handle(SearchFlightsQuery request, CancellationToken cancellationToken)
		{
			// 1. Create the specification
			var spec = new FlightSearchSpecification(
				request.OriginAirportId,
				request.DestinationAirportId,
				request.DepartureDate);

			// 2. Use the existing repository method
			var flights = await _unitOfWork.Repository<OnlineTravel.Domain.Entities.Flights.Flight>()
				.GetAllWithSpecAsync(spec, cancellationToken);

			// 3. Map to DTO
			return flights.Select(f => new SearchFlightsResponse
			{
				FlightId = f.Id,
				FlightNumber = f.FlightNumber.Value,
				CarrierName = f.Carrier.Name,
				CarrierLogo = f.Carrier.Logo ?? "",
				DepartureTime = f.Schedule.Start,
				ArrivalTime = f.Schedule.End
			}).ToList();
		}
	}
}
