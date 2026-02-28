using OnlineTravel.Domain.Enums;

namespace OnlineTravel.Application.Features.Cars.Shared
{
	public class CarSummaryResponse
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
		public string? MainImage { get; set; }
		public decimal PricePerHour { get; set; }
	}
}
