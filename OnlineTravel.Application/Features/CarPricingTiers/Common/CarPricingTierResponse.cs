namespace OnlineTravel.Application.Features.CarPricingTiers.Common
{
	public class CarPricingTierResponse
	{
		public Guid Id { get; set; }
		public Guid CarId { get; set; }
		public int FromHours { get; set; }
		public int ToHours { get; set; }
		public MoneyResponse PricePerHour { get; set; } = null!;
	}
}
