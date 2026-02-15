using NetTopologySuite.Geometries;
using OnlineTravel.Application.Specifications;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Hotels;
using System.Linq.Expressions;


namespace OnlineTravel.Application.Hotels.Specifications
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
            AddInclude("Rooms.Bookings");

            // Build criteria
            var criteria = BuildCriteria(city, minRating, location, radiusInKm);
            if (criteria != null)
                Criteria = criteria;
         

            // Apply sorting
            ApplySorting(sortBy);

            // Apply paging
            ApplyPagination((pageNumber - 1) * pageSize, pageSize);
        }

        private System.Linq.Expressions.Expression<Func<Hotel, bool>>? BuildCriteria(
            string? city,
            int? minRating,
            Point? location,
            double? radiusInKm)
        {
            System.Linq.Expressions.Expression<Func<Hotel, bool>>? criteria = null;

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

        private System.Linq.Expressions.Expression<Func<T, bool>> CombineWithAnd<T>(
            System.Linq.Expressions.Expression<Func<T, bool>> first,
            System.Linq.Expressions.Expression<Func<T, bool>> second)
        {
            var parameter = System.Linq.Expressions.Expression.Parameter(typeof(T));

            var combined = System.Linq.Expressions.Expression.AndAlso(
                System.Linq.Expressions.Expression.Invoke(first, parameter),
                System.Linq.Expressions.Expression.Invoke(second, parameter)
            );

            return System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(combined, parameter);
        }
    }



}
