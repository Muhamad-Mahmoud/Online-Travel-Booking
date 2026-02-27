using MediatR;

namespace OnlineTravel.Application.Features.Flight.Flights.CreateFlight
{
	public class CreateFlightCommand : IRequest<OnlineTravel.Domain.ErrorHandling.Result<Guid>>
	{
		public string FlightNumber { get; set; } = string.Empty;
		public Guid CarrierId { get; set; }
		public Guid OriginAirportId { get; set; }
		public Guid DestinationAirportId { get; set; }

		// Schedule details
		public DateTime DepartureTime { get; set; }
		public DateTime ArrivalTime { get; set; }

		public List<string> BaggageRules { get; set; } = new();
		public string? BaggageRulesText { get; set; }
		public bool Refundable { get; set; }
		public Guid CategoryId { get; set; }

		// Metadata
		public string? Gate { get; set; }
		public string? Terminal { get; set; }
		public string? AircraftType { get; set; }
	}
}
