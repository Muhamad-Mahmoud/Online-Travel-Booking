using OnlineTravel.Domain.Entities.Cars;

namespace OnlineTravel.Application.Specifications.Carspec
{

	public class CarPricingTierSpecification : BaseSpecification<CarPricingTier>
	{
		public CarPricingTierSpecification(Guid? carId = null)
		{
			if (carId.HasValue)
			{
				Criteria = x => x.CarId == carId.Value;
			}
		}

		public static BaseSpecification<CarPricingTier> OverlapSpec(Guid carId, int fromHours, int toHours, Guid? excludeId = null)
		{
			var spec = new BaseSpecification<CarPricingTier>(x =>
				x.CarId == carId &&
				x.FromHours < toHours &&
				fromHours < x.ToHours);

			if (excludeId.HasValue)
			{
				spec.Criteria = spec.Criteria!.And(x => x.Id != excludeId.Value);
			}

			return spec;
		}
	}

}
