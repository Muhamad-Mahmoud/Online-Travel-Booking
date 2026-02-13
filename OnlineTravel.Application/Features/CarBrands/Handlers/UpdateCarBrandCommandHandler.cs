using Domain.ErrorHandling;
using Mapster;
using MediatR;
using OnlineTravel.Application.Features.CarBrands.Commands;
using OnlineTravel.Application.Features.CarBrands.DTOs;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Cars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.CarBrands.Handlers
{



    public class UpdateCarBrandCommandHandler : IRequestHandler<UpdateCarBrandCommand, Result<CarBrandDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCarBrandCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CarBrandDto>> Handle(UpdateCarBrandCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var brand = await _unitOfWork.Repository<CarBrand>().GetByIdAsync(request.Id);
                if (brand is null)
                    return Result<CarBrandDto>.Failure(EntityError<CarBrand>.NotFound());

          var duplicate = await _unitOfWork.Repository<CarBrand>()
                    .FindAsync(b => b.Name == request.Dto.Name && b.Id != request.Id);
                if (duplicate is not null)
                    return Result<CarBrandDto>.Failure(EntityError<CarBrand>.Duplicated("Another brand with same name already exists"));

                request.Dto.Adapt(brand);

          
                _unitOfWork.Repository<CarBrand>().Update(brand);
                await _unitOfWork.Complete();

             
                var resultDto = brand.Adapt<CarBrandDto>();
                return Result<CarBrandDto>.Success(resultDto);
            }
            catch (Exception ex)
            {
                return Result<CarBrandDto>.Failure(EntityError<CarBrand>.OperationFailed(ex.Message));
            }
        }
    }
}
