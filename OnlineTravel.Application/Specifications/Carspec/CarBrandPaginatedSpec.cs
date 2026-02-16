
using OnlineTravel.Domain.Entities.Cars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Specifications.Carspec
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

            // ترتيب افتراضي
            AddOrderBy(x => x.Name);
        }
    }
}
