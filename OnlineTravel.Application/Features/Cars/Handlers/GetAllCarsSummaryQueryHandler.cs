using Mapster;
using MediatR;
using OnlineTravel.Application.Features.Cars.DTOs;
using OnlineTravel.Application.Features.Cars.Queries;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Application.Specifications.Carspec;
using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Cars.Handlers
{
    public class GetAllCarsSummaryQueryHandler : IRequestHandler<GetAllCarsSummaryQuery, Result<PaginatedResult<CarSummaryDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllCarsSummaryQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PaginatedResult<CarSummaryDto>>> Handle(GetAllCarsSummaryQuery request, CancellationToken cancellationToken)
        {
            // بناء الـ Specification مع الفلترة وشمل العلاقات المطلوبة (Brand, Category, PricingTiers للسعر)
            var spec = new CarSpecification(request.BrandId)
                .IncludeBrandAndCategory()
                .IncludePricingTiers() // نحتاج PricingTiers لحساب السعر
                .OrderByCreatedDesc();

            if (request.CategoryId.HasValue)
                spec.WithCategory(request.CategoryId.Value);
            if (request.CarType.HasValue)
                spec.WithCarType(request.CarType.Value);

            spec.ApplyPagination((request.PageIndex - 1) * request.PageSize, request.PageSize);

            var items = await _unitOfWork.Repository<Car>().GetAllWithSpecAsync(spec, cancellationToken);

            // حساب العدد الإجمالي (بدون Includes)
            var countSpec = new CarSpecification(request.BrandId);
            if (request.CategoryId.HasValue)
                countSpec.WithCategory(request.CategoryId.Value);
            if (request.CarType.HasValue)
                countSpec.WithCarType(request.CarType.Value);

            var totalCount = await _unitOfWork.Repository<Car>().GetCountAsync(countSpec, cancellationToken);

            var dtos = items.Adapt<IReadOnlyList<CarSummaryDto>>();
            var paginated = new PaginatedResult<CarSummaryDto>(request.PageIndex, request.PageSize, totalCount, dtos);

            return Result<PaginatedResult<CarSummaryDto>>.Success(paginated);
        }
    }
}
