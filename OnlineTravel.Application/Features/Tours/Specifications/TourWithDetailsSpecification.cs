using OnlineTravel.Domain.Entities.Tours;
using OnlineTravel.Application.Common.Specifications;

namespace OnlineTravel.Application.Features.Tours.Specifications
{
	public class TourWithDetailsSpecification : BaseSpecification<Tour>
	{
		public TourWithDetailsSpecification(Guid id) : base(t => t.Id == id)
		{
			AddIncludes(t => t.Category);
			AddIncludes(t => t.MainImage);
			AddIncludes(t => t.PriceTiers);
			AddIncludes(t => t.Reviews);
			AddIncludes(t => t.Address);
			AddIncludes(t => t.Activities);
			AddIncludes(t => t.Images);
		}
	}
}
