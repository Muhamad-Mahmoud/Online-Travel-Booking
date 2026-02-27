namespace OnlineTravel.Application.Features.Flight.Airport.CreateAirport
{
	public class CreateAirportResponse
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Code { get; set; } = string.Empty;
	}
}
