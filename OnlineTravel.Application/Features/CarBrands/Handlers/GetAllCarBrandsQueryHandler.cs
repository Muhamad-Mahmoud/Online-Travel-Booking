using Domain.ErrorHandling;
using Mapster;
using MediatR;
using OnlineTravel.Application.Features.CarBrands.DTOs;
using OnlineTravel.Application.Features.CarBrands.Queries;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Cars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.CarBrands.Handlers
{
    public class GetAllCarBrandsQueryHandler : IRequestHandler<GetAllCarBrandsQuery, Result<IReadOnlyList<CarBrandDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllCarBrandsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IReadOnlyList<CarBrandDto>>> Handle(GetAllCarBrandsQuery request, CancellationToken cancellationToken)
        {
            var brands = await _unitOfWork.Repository<CarBrand>().GetAllAsync();
            var dtos = brands.Adapt<IReadOnlyList<CarBrandDto>>();
            return Result<IReadOnlyList<CarBrandDto>>.Success(dtos);
        }
    }
}
