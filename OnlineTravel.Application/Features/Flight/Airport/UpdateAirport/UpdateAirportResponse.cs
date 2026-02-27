namespace OnlineTravel.Application.Features.Flight.Airport.UpdateAirport
{
	public class UpdateAirportResponse
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public bool IsSuccess { get; set; }
	}
}
