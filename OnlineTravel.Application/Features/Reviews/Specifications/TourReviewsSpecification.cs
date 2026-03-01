using OnlineTravel.Application.Common.Specifications;
using OnlineTravel.Domain.Entities.Reviews;

namespace OnlineTravel.Application.Features.Reviews.Specifications;

public class TourReviewsSpecification : BaseSpecification<Review>
{
	public TourReviewsSpecification(Guid tourId, Guid categoryId)
		: base(r => r.ItemId == tourId && r.CategoryId == categoryId)
	{
		AddIncludes(r => r.User);
		AddOrderByDesc(r => r.CreatedAt);
	}
}
