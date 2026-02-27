using System.ComponentModel.DataAnnotations;
using OnlineTravel.Application.Features.Cars.Shared.DTOs;
using OnlineTravel.Domain.Enums;

namespace OnlineTravel.Application.Features.Cars.CreateCar
{
	public class CreateCarRequest
	{
		[Required]
		public Guid BrandId { get; set; }

		[Required]
		[StringLength(100, MinimumLength = 2)]
		public string Make { get; set; } = string.Empty;

		[Required]
		[StringLength(100, MinimumLength = 1)]
		public string Model { get; set; } = string.Empty;

		[Required]
		public CarCategory CarType { get; set; }

		[Range(1, 50)]
		public int SeatsCount { get; set; }

		[Required]
		public FuelType FuelType { get; set; }

		[Required]
		public TransmissionType Transmission { get; set; }

		public List<string> Features { get; set; } = new();

		public List<DateTimeRangeDto> AvailableDates { get; set; } = new();

		[StringLength(500)]
		public string? CancellationPolicy { get; set; }

		[Required]
		public Guid CategoryId { get; set; }

		[Required]
		public LocationDto Location { get; set; } = null!;

		public List<ImageUrlDto> Images { get; set; } = new();
	}
}
