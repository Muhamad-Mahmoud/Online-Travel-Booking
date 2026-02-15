
using Mapster;
using MediatR;

using OnlineTravel.Application.Features.CarBrands.DTOs;
using OnlineTravel.Application.Features.CarBrands.Queries;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Application.Specifications;
using OnlineTravel.Application.Specifications.Carspec;
using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.CarBrands.Handlers
{

    public class GetCarBrandsPaginatedQueryHandler : IRequestHandler<GetCarBrandsPaginatedQuery, Result<PaginatedResult<CarBrandDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCarBrandsPaginatedQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PaginatedResult<CarBrandDto>>> Handle(
       GetCarBrandsPaginatedQuery request,
       CancellationToken cancellationToken)
        {

            var pagedSpec = new CarBrandPaginatedSpec(request.PageIndex, request.PageSize, request.SearchTerm);
            var brands = await _unitOfWork.Repository<CarBrand>().GetAllWithSpecAsync(pagedSpec);

            var countSpec = new BaseSpecification<CarBrand>();
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                countSpec.Criteria = x => x.Name.Contains(request.SearchTerm);
            

            var totalCount = await _unitOfWork.Repository<CarBrand>().GetCountAsync(countSpec);

            var dtos = brands.Adapt<IReadOnlyList<CarBrandDto>>();

            var paginatedResult = new PaginatedResult<CarBrandDto>(
                request.PageIndex,
                request.PageSize,
                totalCount,
                dtos);

            return Result<PaginatedResult<CarBrandDto>>.Success(paginatedResult);
        }
    }
}
