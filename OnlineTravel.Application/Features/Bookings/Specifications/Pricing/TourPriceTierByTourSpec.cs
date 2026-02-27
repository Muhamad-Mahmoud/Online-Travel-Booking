using OnlineTravel.Application.Common.Specifications;
using OnlineTravel.Domain.Entities.Tours;

namespace OnlineTravel.Application.Features.Bookings.Specifications.Pricing;

public class TourPriceTierByTourSpec : BaseSpecification<TourPriceTier>
{
	public TourPriceTierByTourSpec(Guid tourId)
		: base(t => t.TourId == tourId)
	{
	}
}
