using OnlineTravel.Application.Specifications;
using OnlineTravel.Domain.Entities.Tours;

namespace OnlineTravel.Application.Features.Tours.Specifications;

public class TourWithDetailsSpecification : BaseSpecification<Tour>
{
    public TourWithDetailsSpecification(Guid id) : base(t => t.Id == id)
    {
        AddIncludes(t => t.Category);
        AddIncludes(t => t.Activities);
        AddIncludes(t => t.PriceTiers);
        AddIncludes(t => t.Images);
    }
}
