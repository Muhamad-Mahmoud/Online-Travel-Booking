namespace OnlineTravel.Application.Features.Cars.Shared
{
	public class LocationResponse
	{
		public string AddressLine { get; set; } = string.Empty;
		public string City { get; set; } = string.Empty;
		public string State { get; set; } = string.Empty;
		public string Country { get; set; } = string.Empty;
		public string ZipCode { get; set; } = string.Empty;
		public double Latitude { get; set; }
		public double Longitude { get; set; }
	}

	public class ImageUrlResponse
	{
		public Guid Id { get; set; }
		public string Url { get; set; } = string.Empty;
		public string AltText { get; set; } = string.Empty;
	}

	public class DateTimeRangeResponse
	{
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
	}
}
