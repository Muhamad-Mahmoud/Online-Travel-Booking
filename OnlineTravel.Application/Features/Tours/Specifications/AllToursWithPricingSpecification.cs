using OnlineTravel.Application.Specifications;
using OnlineTravel.Domain.Entities.Tours;

namespace OnlineTravel.Application.Features.Tours.Specifications;

public class AllToursWithPricingSpecification : BaseSpecification<Tour>
{
    public AllToursWithPricingSpecification(string? search) 
        : base(x => string.IsNullOrEmpty(search) || 
                    x.Title.Contains(search) || 
                    x.Address.City != null && x.Address.City.Contains(search) || 
                    x.Address.Country != null && x.Address.Country.Contains(search) ||
                    x.Activities.Any(a => a.Title.Contains(search)))
    {
        AddIncludes(t => t.Category);
        AddIncludes(t => t.PriceTiers);
        AddOrderBy(t => t.Title);
    }
}
