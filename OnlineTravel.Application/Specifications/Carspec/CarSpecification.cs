using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.Enums;
using OnlineTravel.Application.Common.Extensions;
using System;
using System.Linq.Expressions;

namespace OnlineTravel.Application.Specifications.Carspec
{
    public class CarSpecification : BaseSpecification<Car>
    {
        public CarSpecification()
        {
            Criteria = x => x.DeletedAt == null;
        }

        public CarSpecification(Guid? brandId = null) : this()
        {
            if (brandId.HasValue)
                Criteria = Criteria.AndAlso(x => x.BrandId == brandId.Value);
        }

        public CarSpecification WithCategory(Guid categoryId)
        {
            Criteria = Criteria.AndAlso(x => x.CategoryId == categoryId);
            return this;
        }

        public CarSpecification WithCarType(CarCategory carType)
        {
            Criteria = Criteria.AndAlso(x => x.CarType == carType);
            return this;
        }

        public CarSpecification WithSearchTerm(string? searchTerm)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.ToLower();
                Criteria = Criteria.AndAlso(x => x.Make.ToLower().Contains(term) || x.Model.ToLower().Contains(term));
            }
            return this;
        }

        public CarSpecification IncludeBrandAndCategory()
        {
            AddIncludes(x => x.Brand);
            AddIncludes(x => x.Category);
            return this;
        }

        public CarSpecification IncludePricingTiers()
        {
            AddIncludes(x => x.PricingTiers);
            return this;
        }

        public CarSpecification ApplyPagination(int skip, int take)
        {
            base.ApplyPagination(skip, take);
            return this;
        }

        public CarSpecification OrderByCreatedDesc()
        {
            AddOrderByDesc(x => x.CreatedAt);
            return this;
        }
    }
}

namespace OnlineTravel.Application.Common.Extensions
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> AndAlso<T>(
            this Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(left.Parameters[0], parameter);
            var leftExpr = leftVisitor.Visit(left.Body);

            var rightVisitor = new ReplaceExpressionVisitor(right.Parameters[0], parameter);
            var rightExpr = rightVisitor.Visit(right.Body);

            var andExpr = Expression.AndAlso(leftExpr, rightExpr);
            return Expression.Lambda<Func<T, bool>>(andExpr, parameter);
        }

        private class ReplaceExpressionVisitor : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                return node == _oldValue ? _newValue : base.Visit(node);
            }
        }
    }
}
