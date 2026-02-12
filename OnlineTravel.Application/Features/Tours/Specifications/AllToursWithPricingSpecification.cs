using OnlineTravel.Application.Specifications;
using OnlineTravel.Domain.Entities.Tours;

namespace OnlineTravel.Application.Features.Tours.Specifications;

public class AllToursWithPricingSpecification : BaseSpecification<Tour>
{
    public AllToursWithPricingSpecification() : base()
    {
        AddIncludes(t => t.Category);
        AddIncludes(t => t.PriceTiers);
        AddOrderBy(t => t.Title);
    }
}
