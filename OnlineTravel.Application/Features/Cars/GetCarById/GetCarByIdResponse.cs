using OnlineTravel.Application.Features.Cars.Shared;
using OnlineTravel.Domain.Enums;

namespace OnlineTravel.Application.Features.Cars.GetCarById
{
	public class GetCarByIdResponse
	{
		public Guid Id { get; set; }
		public Guid BrandId { get; set; }
		public string? BrandName { get; set; }
		public string Make { get; set; } = string.Empty;
		public string Model { get; set; } = string.Empty;
		public CarCategory CarType { get; set; }
		public int SeatsCount { get; set; }
		public FuelType FuelType { get; set; }
		public TransmissionType Transmission { get; set; }
		public List<string> Features { get; set; } = new();
		public List<DateTimeRangeResponse> AvailableDates { get; set; } = new();
		public string? CancellationPolicy { get; set; }
		public Guid CategoryId { get; set; }
		public string? CategoryTitle { get; set; }
		public LocationResponse Location { get; set; } = null!;
		public List<ImageUrlResponse> Images { get; set; } = new();
	}
}

