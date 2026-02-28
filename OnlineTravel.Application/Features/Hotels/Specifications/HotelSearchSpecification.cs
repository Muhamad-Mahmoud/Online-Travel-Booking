using System.Linq.Expressions;
using NetTopologySuite.Geometries;
using OnlineTravel.Application.Common.Specifications;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Hotels;


namespace OnlineTravel.Application.Features.Hotels.Specifications
{
	public class HotelSearchSpecification : BaseSpecification<Hotel>
	{
		public HotelSearchSpecification(
		 string? city = null,
		 DateRange? dateRange = null,
		 int? minRating = null,
		 decimal? minPrice = null,
		 decimal? maxPrice = null,
		 Point? location = null,
		 double? radiusInKm = null,
		 int pageNumber = 1,
		 int pageSize = 10,
		 string? sortBy = "name")
		{
			// Always include navigation properties with eager loading
			AddIncludes(h => h.Rooms);
			AddIncludes(h => h.Reviews);
			AddInclude("Rooms.SeasonalPrices");
			AddInclude("Rooms.RoomAvailabilities");

			// Build criteria
			var criteria = BuildCriteria(city, minRating, location, radiusInKm);
			if (criteria != null)
				Criteria = criteria;


			// Apply sorting
			ApplySorting(sortBy);

			// Apply paging
			ApplyPagination((pageNumber - 1) * pageSize, pageSize);
		}

		private Expression<Func<Hotel, bool>>? BuildCriteria(
			string? city,
			int? minRating,
			Point? location,
			double? radiusInKm)
		{
			Expression<Func<Hotel, bool>>? criteria = null;

			if (!string.IsNullOrWhiteSpace(city))
			{
				criteria = h => h.Address.City.ToLower().Contains(city.ToLower());
			}

			if (minRating.HasValue)
			{
				Expression<Func<Hotel, bool>> ratingCriteria = h => h.Rating != null && h.Rating.Value >= minRating.Value;


				criteria = criteria == null ? ratingCriteria : CombineWithAnd(criteria, ratingCriteria);
			}

			if (location != null && radiusInKm.HasValue)
			{
				var radiusInMeters = radiusInKm.Value * 1000;
				Expression<Func<Hotel, bool>> radiusCriteria = h => h.Address.Coordinates != null && h.Address.Coordinates.Distance(location) <= radiusInMeters;
				criteria = criteria == null ? radiusCriteria : CombineWithAnd(criteria, radiusCriteria);
			}

			return criteria;
		}

		private void ApplySorting(string? sortBy)
		{
			switch (sortBy?.ToLower())
			{
				case "rating":
					AddOrderByDesc(h => h.Rating != null ? h.Rating.Value : 0);
					break;
				case "name":
				default:
					AddOrderBy(h => h.Name);
					break;
			}
		}

		private Expression<Func<T, bool>> CombineWithAnd<T>(
			Expression<Func<T, bool>> first,
			Expression<Func<T, bool>> second)
		{
			var parameter = Expression.Parameter(typeof(T));

			var combined = Expression.AndAlso(
				Expression.Invoke(first, parameter),
				Expression.Invoke(second, parameter)
			);

			return Expression.Lambda<Func<T, bool>>(combined, parameter);
		}
	}



}
