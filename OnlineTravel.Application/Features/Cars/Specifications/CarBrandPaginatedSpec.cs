using OnlineTravel.Application.Common.Specifications;
using OnlineTravel.Domain.Entities.Cars;

namespace OnlineTravel.Application.Features.Cars.Specifications
{
	public class CarBrandPaginatedSpec : BaseSpecification<CarBrand>
	{
		public CarBrandPaginatedSpec(int pageIndex, int pageSize, string? searchTerm)
		{
			// Pagination
			ApplyPagination((pageIndex - 1) * pageSize, pageSize);

			// Search
			if (!string.IsNullOrWhiteSpace(searchTerm))
			{
				Criteria = x => x.Name.Contains(searchTerm);
			}

			// ????? ???????
			AddOrderBy(x => x.Name);
		}
	}
}
