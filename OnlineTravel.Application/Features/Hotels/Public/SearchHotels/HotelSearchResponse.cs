namespace OnlineTravel.Application.Features.Hotels.Public.SearchHotels
{

	public class HotelSearchResponse
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Slug { get; set; }
		public string Description { get; set; }
		public string MainImage { get; set; }
		public decimal Rating { get; set; }
		public string City { get; set; }
		public string Country { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public decimal MinPrice { get; set; }
		public int TotalRooms { get; set; }
	}


}
