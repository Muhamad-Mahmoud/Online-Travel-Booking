using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Specifications.Carspec
{
    public class CarSpecification : BaseSpecification<Car>
    {
        // فلترة حسب العلامة التجارية
        public CarSpecification(Guid? brandId = null)
        {
            if (brandId.HasValue)
                Criteria = x => x.BrandId == brandId.Value;
        }

        // فلترة حسب التصنيف
        public CarSpecification WithCategory(Guid categoryId)
        {
            Criteria = Criteria == null
                ? x => x.CategoryId == categoryId
                : Criteria.And(x => x.CategoryId == categoryId);
            return this;
        }

        // فلترة حسب نوع السيارة
        public CarSpecification WithCarType(CarCategory carType)
        {
            Criteria = Criteria == null
                ? x => x.CarType == carType
                : Criteria.And(x => x.CarType == carType);
            return this;
        }

        // تضمين العلاقات
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

        // تطبيق Pagination (مصححة)
        public CarSpecification ApplyPagination(int skip, int take)
        {
            base.ApplyPagination(skip, take); // استدعاء دالة pagination من BaseSpecification
            return this;
        }

        // ترتيب حسب التاريخ
        public CarSpecification OrderByCreatedDesc()
        {
            AddOrderByDesc(x => x.CreatedAt);
            return this;
        }
    }
}
