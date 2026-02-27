using OnlineTravel.Application.Specifications;
using OnlineTravel.Domain.Entities.Cars;

namespace OnlineTravel.Application.Features.Bookings.Specifications.Pricing;

public class CarPricingTierByCarSpec : BaseSpecification<CarPricingTier>
{
	public CarPricingTierByCarSpec(Guid carId)
		: base(c => c.CarId == carId)
	{
	}
}
