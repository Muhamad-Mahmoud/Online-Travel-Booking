using System.Linq.Expressions;

namespace OnlineTravel.Application.Interfaces.Specifications
{
    public interface ISpecification<T>
    {

        Expression<Func<T, bool>>? Criteria { get; set; }
        List<Expression<Func<T, object>>> Includes { get; }
        int Take { get; set; }
        int Skip { get; set; }
        bool IsPaginationEnabled { get; set; }
        Expression<Func<T, object>>? OrderBy { get; set; }
        Expression<Func<T, object>>? OrderByDescending { get; set; }
    }
}
