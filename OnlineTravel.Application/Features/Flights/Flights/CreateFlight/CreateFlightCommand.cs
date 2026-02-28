using MediatR;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Flights.Flights.CreateFlight
{
	public class CreateFlightCommand : IRequest<Result<Guid>>
	{
		public string FlightNumber { get; set; } = string.Empty;
		public Guid CarrierId { get; set; }
		public Guid OriginAirportId { get; set; }
		public Guid DestinationAirportId { get; set; }
		public DateTime DepartureTime { get; set; }
		public DateTime ArrivalTime { get; set; }
		public string? BaggageRules { get; set; }
		public bool Refundable { get; set; }
		public Guid CategoryId { get; set; }
		public string? Gate { get; set; }
		public string? Terminal { get; set; }
		public string? AircraftType { get; set; }
	}
}
