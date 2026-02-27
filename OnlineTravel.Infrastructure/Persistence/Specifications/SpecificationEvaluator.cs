using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OnlineTravel.Application.Interfaces.Specifications;
using OnlineTravel.Domain.Entities._Base;

namespace OnlineTravel.Infrastructure.Persistence.Specifications
{
	public static class SpecificationEvaluator<T> where T : BaseEntity
	{
		public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
		{
			var query = inputQuery;

			if (spec.Criteria is not null)
				query = query.Where(spec.Criteria);

			query = spec.Includes.Aggregate(query, (current, include) =>
			{
				if (include.Body is MethodCallExpression methodCall && methodCall.Method.Name == "Select")
				{
					// Convert b => b.Details.Select(d => d.Category) TO "Details.Category"
					try
					{
						var path = ParseIncludePath(include);
						return current.Include(path);
					}
					catch
					{
						// Fallback if parsing fails (though likely to throw anyway)
						return current.Include(include);
					}
				}
				return current.Include(include);
			});

			query = spec.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));

			if (spec.OrderBy is not null)
				query = query.OrderBy(spec.OrderBy);

			if (spec.OrderByDescending is not null)
				query = query.OrderByDescending(spec.OrderByDescending);

			if (spec.IsPaginationEnabled)
				query = query.Skip(spec.Skip).Take(spec.Take);

			return query;

		}

		private static string ParseIncludePath(Expression expression)
		{
			var path = string.Empty;
			if (expression is LambdaExpression lambda)
			{
				expression = lambda.Body;
			}

			if (expression is MethodCallExpression methodCall && methodCall.Method.Name == "Select")
			{
				// Recursive for Details
				var parent = methodCall.Arguments[0];
				var child = methodCall.Arguments[1]; // d => d.Category

				var parentPath = ParseIncludePath(parent);
				var childPath = ParseIncludePath(child);

				return string.IsNullOrEmpty(parentPath) ? childPath : $"{parentPath}.{childPath}";
			}
			else if (expression is MemberExpression member)
			{
				var parent = member.Expression;
				var current = member.Member.Name;

				if (parent is ParameterExpression)
					return current;

				var parentPath = ParseIncludePath(parent);
				return string.IsNullOrEmpty(parentPath) ? current : $"{parentPath}.{current}";
			}

			return string.Empty;
		}
	}
}
