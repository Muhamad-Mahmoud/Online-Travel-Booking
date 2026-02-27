using System.ComponentModel.DataAnnotations;

namespace OnlineTravel.Application.Features.CarPricingTiers.Create
{
	public class CreateCarPricingTierFormModel
	{
		[Required]
		public Guid CarId { get; set; }

		[Range(0, int.MaxValue, ErrorMessage = "FromHours must be >= 0")]
		public int FromHours { get; set; }

		[Range(1, int.MaxValue, ErrorMessage = "ToHours must be > 0")]
		public int ToHours { get; set; }

		[Required]
		public MoneyFormModel PricePerHour { get; set; } = null!;
	}

	public class MoneyFormModel
	{
		public decimal Amount { get; set; }
		public string Currency { get; set; } = "USD";
	}
}
