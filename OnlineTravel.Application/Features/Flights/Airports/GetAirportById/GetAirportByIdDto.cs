namespace OnlineTravel.Application.Features.Flights.Airport.GetAirportById
{
	public class GetAirportByIdDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public string FullAddress { get; set; } // We can combine address fields here
		public string City { get; set; }
		public string Country { get; set; }
		public List<string> Facilities { get; set; }
	}
}
