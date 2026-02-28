using OnlineTravel.Application.Features.CarPricingTiers.Common;
using OnlineTravel.Domain.Enums;

namespace OnlineTravel.Application.Features.Cars.Shared
{
	public class CarDetailsResponse
	{
		public Guid Id { get; set; }
		public string Make { get; set; } = string.Empty;
		public string Model { get; set; } = string.Empty;
		public string? BrandName { get; set; }
		public string? CategoryTitle { get; set; }
		public CarCategory CarType { get; set; }
		public int SeatsCount { get; set; }
		public FuelType FuelType { get; set; }
		public TransmissionType Transmission { get; set; }
		public List<string> Features { get; set; } = new();
		public List<DateTimeRangeResponse> AvailableDates { get; set; } = new();
		public string? CancellationPolicy { get; set; }
		public LocationResponse Location { get; set; } = null!;
		public List<ImageUrlResponse> Images { get; set; } = new();
		public List<CarPricingTierResponse> PricingTiers { get; set; } = new();

		public string? MainImage { get; set; }
		public decimal PricePerHour { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
	}
}
