using NetTopologySuite.Geometries;
using OnlineTravel.Application.Specifications;
using OnlineTravel.Domain.Entities.Tours;

namespace OnlineTravel.Application.Features.Tours.Specifications;

public class AllToursWithPricingSpecification : BaseSpecification<Tour>
{
	public AllToursWithPricingSpecification(string? search, double? lat, double? lon, double? radiusKm, decimal? minPrice, decimal? maxPrice, int? rating, string? city, string? country, string? sortOrder)
		: base(x => (string.IsNullOrEmpty(search) ||
					 x.Title.Contains(search) ||
					 x.Activities.Any(a => a.Title.Contains(search))) &&
					(string.IsNullOrEmpty(city) || (x.Address.City != null && x.Address.City.Contains(city))) &&
					(string.IsNullOrEmpty(country) || (x.Address.Country != null && x.Address.Country.Contains(country))) &&
					(!lat.HasValue || !lon.HasValue || !radiusKm.HasValue ||
					 (x.Address.Coordinates != null && x.Address.Coordinates.IsWithinDistance(new Point(lon.Value, lat.Value) { SRID = 4326 }, radiusKm.Value * 1000))) &&
					(!minPrice.HasValue || x.PriceTiers.Any(p => p.Price.Amount >= minPrice.Value)) &&
					(!maxPrice.HasValue || x.PriceTiers.Any(p => p.Price.Amount <= maxPrice.Value)) &&
					(!rating.HasValue || (x.Reviews.Any() && x.Reviews.Average(r => r.Rating.Value) >= rating.Value && x.Reviews.Average(r => r.Rating.Value) < rating.Value + 1)))
	{
		AddIncludes(t => t.Category);
		AddIncludes(t => t.PriceTiers);
		AddIncludes(t => t.Reviews);

		if (!string.IsNullOrEmpty(sortOrder))
		{
			switch (sortOrder.ToLower())
			{
				case "price_asc":
					AddOrderBy(t => t.PriceTiers.Min(p => p.Price.Amount));
					break;
				case "price_desc":
					AddOrderByDesc(t => t.PriceTiers.Min(p => p.Price.Amount));
					break;
				case "most_reviewed":
					AddOrderByDesc(t => t.Reviews.Count());
					break;
				case "top_rated":
					AddOrderByDesc(t => t.Reviews.Average(r => r.Rating.Value));
					break;
				default:
					AddOrderBy(t => t.Title);
					break;
			}
		}
		else
		{
			AddOrderBy(t => t.Title);
		}
	}
}
