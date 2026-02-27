namespace OnlineTravel.Application.Features.Flight.Airport.GetAllAirports
{
	public class GetAllAirportsDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Code { get; set; } = string.Empty;
		public string City { get; set; } = string.Empty;
		public string Country { get; set; } = string.Empty;
	}
}
