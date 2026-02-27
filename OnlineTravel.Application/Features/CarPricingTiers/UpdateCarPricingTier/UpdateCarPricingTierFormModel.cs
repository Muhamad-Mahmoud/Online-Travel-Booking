using System.ComponentModel.DataAnnotations;

namespace OnlineTravel.Application.Features.CarPricingTiers.Update
{
	public class UpdateCarPricingTierFormModel
	{
		[Required]
		public Guid Id { get; set; }

		[Required]
		public Guid CarId { get; set; }

		[Range(0, int.MaxValue)]
		public int FromHours { get; set; }

		[Range(1, int.MaxValue)]
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
