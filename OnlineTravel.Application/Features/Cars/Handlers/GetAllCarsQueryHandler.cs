using Mapster;
using MediatR;
using OnlineTravel.Application.Features.Cars.DTOs;
using OnlineTravel.Application.Features.Cars.Queries;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Application.Specifications.Carspec;
using OnlineTravel.Application.Specifications.Carspec.OnlineTravel.Application.Specifications;
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

    public class GetAllCarsQueryHandler : IRequestHandler<GetAllCarsQuery, Result<PaginatedResult<CarDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllCarsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PaginatedResult<CarDto>>> Handle(GetAllCarsQuery request, CancellationToken cancellationToken)
        {
            // بناء الـ Specification
            var spec = new CarSpecification(request.BrandId)
                .IncludeBrandAndCategory()
                .OrderByCreatedDesc();

            if (request.CategoryId.HasValue)
                spec.WithCategory(request.CategoryId.Value);

            if (request.CarType.HasValue)
                spec.WithCarType(request.CarType.Value);

            // Pagination
            spec.ApplyPagination((request.PageIndex - 1) * request.PageSize, request.PageSize);

            // جلب البيانات
            var items = await _unitOfWork.Repository<Car>()
                .GetAllWithSpecAsync(spec, cancellationToken);

            // حساب العدد الإجمالي
            var countSpec = new CarSpecification(request.BrandId);
            if (request.CategoryId.HasValue)
                countSpec.WithCategory(request.CategoryId.Value);
            if (request.CarType.HasValue)
                countSpec.WithCarType(request.CarType.Value);

            var totalCount = await _unitOfWork.Repository<Car>()
                .GetCountAsync(countSpec, cancellationToken);

            var dtos = items.Adapt<IReadOnlyList<CarDto>>();
            var paginated = new PaginatedResult<CarDto>(request.PageIndex, request.PageSize, totalCount, dtos);

            return Result<PaginatedResult<CarDto>>.Success(paginated);
        }
    }
}
