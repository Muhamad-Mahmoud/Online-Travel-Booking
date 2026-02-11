using OnlineTravel.Domain.Entities.Cars;

namespace OnlineTravel.Application.Specifications;

public class CarPricingTierByCarSpec : BaseSpecification<CarPricingTier>
{
    public CarPricingTierByCarSpec(Guid carId)
        : base(c => c.CarId == carId)
    {
    }
}
