using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Application.Specifications;

namespace OnlineTravel.Application.Features.Bookings.Specifications.Pricing;

public class CarPricingTierByCarSpec : BaseSpecification<CarPricingTier>
{
    public CarPricingTierByCarSpec(Guid carId)
        : base(c => c.CarId == carId)
    {
    }
}
