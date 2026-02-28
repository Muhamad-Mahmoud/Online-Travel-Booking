namespace OnlineTravel.Application.Features.Flights.Airport.CreateAirport
{
	public class CreateAirportResponse
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Code { get; set; } = string.Empty;
	}
}
