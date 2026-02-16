using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Specifications.Carspec
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> And<T>(
            this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            var param = Expression.Parameter(typeof(T));

            var body = Expression.AndAlso(
                Expression.Invoke(first, param),
                Expression.Invoke(second, param)
            );

            return Expression.Lambda<Func<T, bool>>(body, param);
        }
    }

}
