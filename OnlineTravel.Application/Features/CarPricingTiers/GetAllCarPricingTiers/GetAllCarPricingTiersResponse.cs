namespace OnlineTravel.Application.Features.CarPricingTiers.GetAllCarPricingTiers
{
	public class GetAllCarPricingTiersResponse
	{
		public Guid Id { get; set; }
		public Guid CarId { get; set; }
		public int FromHours { get; set; }
		public int ToHours { get; set; }
		public MoneyResponse PricePerHour { get; set; } = null!;
	}

	public class MoneyResponse
	{
		public decimal Amount { get; set; }
		public string Currency { get; set; } = "USD";
	}
}
