using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Specifications.Carspec
{
    using global::OnlineTravel.Application.Specifications.Carspec.OnlineTravel.Application.Common.Extensions;
    using System;
    using System.Linq.Expressions;

    namespace OnlineTravel.Application.Specifications
    {
        public class CarSpecification : BaseSpecification<Car>
        {
            // منشئ أساسي يضيف شرط عدم الحذف (Soft Delete)
            public CarSpecification()
            {
                Criteria = x => x.DeletedAt == null;
            }

            // منشئ مع فلترة حسب العلامة التجارية
            public CarSpecification(Guid? brandId = null) : this()
            {
                if (brandId.HasValue)
                    Criteria = Criteria.AndAlso(x => x.BrandId == brandId.Value);
            }

            // فلترة حسب التصنيف
            public CarSpecification WithCategory(Guid categoryId)
            {
                Criteria = Criteria.AndAlso(x => x.CategoryId == categoryId);
                return this;
            }

            // فلترة حسب نوع السيارة
            public CarSpecification WithCarType(CarCategory carType)
            {
                Criteria = Criteria.AndAlso(x => x.CarType == carType);
                return this;
            }

            // تضمين العلاقات (Brand و Category)
            public CarSpecification IncludeBrandAndCategory()
            {
                AddIncludes(x => x.Brand);
                AddIncludes(x => x.Category);
                return this;
            }

            // تضمين PricingTiers
            public CarSpecification IncludePricingTiers()
            {
                AddIncludes(x => x.PricingTiers);
                return this;
            }

            // تطبيق Pagination
            public CarSpecification ApplyPagination(int skip, int take)
            {
                base.ApplyPagination(skip, take);
                return this;
            }

            // ترتيب حسب التاريخ تنازلياً
            public CarSpecification OrderByCreatedDesc()
            {
                AddOrderByDesc(x => x.CreatedAt);
                return this;
            }
        }
    }

    // نضع الـ Extension Method في namespace منفصل لتجنب التضارب
    namespace OnlineTravel.Application.Common.Extensions
    {
        using System;
        using System.Linq.Expressions;

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
}
