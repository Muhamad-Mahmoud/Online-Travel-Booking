using OnlineTravel.Domain.Entities.Tours;

namespace OnlineTravel.Application.Specifications;

public class TourPriceTierByTourSpec : BaseSpecification<TourPriceTier>
{
    public TourPriceTierByTourSpec(Guid tourId)
        : base(t => t.TourId == tourId)
    {
    }
}
