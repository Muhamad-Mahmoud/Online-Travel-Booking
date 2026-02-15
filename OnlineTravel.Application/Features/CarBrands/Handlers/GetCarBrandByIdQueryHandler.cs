
using Mapster;
using MediatR;
using OnlineTravel.Application.Features.CarBrands.DTOs;
using OnlineTravel.Application.Features.CarBrands.Queries;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.CarBrands.Handlers
{
    public class GetCarBrandByIdQueryHandler : IRequestHandler<GetCarBrandByIdQuery, Result<CarBrandDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCarBrandByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CarBrandDto>> Handle(GetCarBrandByIdQuery request, CancellationToken cancellationToken)
        {
            var brand = await _unitOfWork.Repository<CarBrand>().GetByIdAsync(request.Id);
            if (brand is null)
                return Result<CarBrandDto>.Failure(EntityError<CarBrand>.NotFound());

            var dto = brand.Adapt<CarBrandDto>();
            return Result<CarBrandDto>.Success(dto);
        }
    }
}
