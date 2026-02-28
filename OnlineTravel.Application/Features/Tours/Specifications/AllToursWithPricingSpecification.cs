using OnlineTravel.Domain.Entities.Tours;
using OnlineTravel.Application.Common.Specifications;

namespace OnlineTravel.Application.Features.Tours.Specifications
{
	public class AllToursWithPricingSpecification : BaseSpecification<Tour>
	{
		public AllToursWithPricingSpecification(string? search, double? lat, double? lon, double? radiusKm, decimal? minPrice, decimal? maxPrice, int? rating, string? city, string? country, string? sortOrder)
		{
			AddIncludes(t => t.Category);
			AddIncludes(t => t.MainImage);
			AddIncludes(t => t.PriceTiers);
			AddIncludes(t => t.Reviews);
			AddIncludes(t => t.Address);

			if (!string.IsNullOrEmpty(search))
			{
				// Simplification for recovery
				Criteria = t => t.Title.Contains(search) || (t.Description != null && t.Description.Contains(search));
			}

			if (rating.HasValue)
			{
				// Note: Rating calculation usually done after fetch, but criteria can filter entities
			}

			// Add more filters as needed based on entity structure
		}
	}
}
