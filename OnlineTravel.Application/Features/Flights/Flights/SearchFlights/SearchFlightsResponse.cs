namespace OnlineTravel.Application.Features.Flights.Flights.SearchFlights.DTOs
{
	public class SearchFlightsResponse
	{
		public Guid FlightId { get; set; }
		public string FlightNumber { get; set; } = string.Empty;
		public string CarrierName { get; set; } = string.Empty;
		public string CarrierLogo { get; set; } = string.Empty;
		public DateTime DepartureTime { get; set; }
		public DateTime ArrivalTime { get; set; }
	}
}
