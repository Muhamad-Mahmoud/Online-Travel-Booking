
namespace OnlineTravel.Application.Features.Hotels.Shared

{
	public class HotelDetailsResponse
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Slug { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string MainImage { get; set; } = string.Empty;
		public List<string> Gallery { get; set; } = new();
		public double Rating { get; set; }
		public int ReviewCount { get; set; }
		public string Street { get; set; } = string.Empty;
		public string City { get; set; } = string.Empty;
		public string State { get; set; } = string.Empty;
		public string Country { get; set; } = string.Empty;
		public string PostalCode { get; set; } = string.Empty;
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public decimal BasePrice { get; set; }
		public string Currency { get; set; } = "USD";
		public string CancellationPolicy { get; set; } = string.Empty;
		public string ContactPhone { get; set; } = string.Empty;
		public string ContactEmail { get; set; } = string.Empty;
		public string Website { get; set; } = string.Empty;
		public List<ReviewResponse> Reviews { get; set; } = new();
		public List<RoomResponse> Rooms { get; set; } = new();
		public string CheckInTime { get; set; } = string.Empty;
		public string CheckOutTime { get; set; } = string.Empty;
	}

}
