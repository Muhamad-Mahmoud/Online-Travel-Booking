using MediatR;
using OnlineTravel.Application.Features.Flight.Flights.SearchFlights.DTOs;

namespace OnlineTravel.Application.Features.Flight.Flights.SearchFlights.Queries
{
	public class SearchFlightsQuery : IRequest<List<SearchFlightsDto>>
	{
		public Guid OriginAirportId { get; set; }
		public Guid DestinationAirportId { get; set; }
		public DateTime DepartureDate { get; set; }
		public int Passengers { get; set; } = 1;
	}
}
