using System.Linq.Expressions;

namespace OnlineTravel.Application.Interfaces.Specifications
{
	public interface ISpecification<T>
	{

		Expression<Func<T, bool>>? Criteria { get; }
		List<Expression<Func<T, object>>> Includes { get; }
		List<string> IncludeStrings { get; }

		int Take { get; }
		int Skip { get; }
		bool IsPaginationEnabled { get; }
		Expression<Func<T, object>>? OrderBy { get; }
		Expression<Func<T, object>>? OrderByDescending { get; }
	}
}
