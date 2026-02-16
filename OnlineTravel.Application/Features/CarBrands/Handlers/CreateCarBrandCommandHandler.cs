
using Mapster;
using MediatR;
using OnlineTravel.Application.Features.CarBrands.Commands;
using OnlineTravel.Application.Features.CarBrands.DTOs;
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
    public class CreateCarBrandCommandHandler : IRequestHandler<CreateCarBrandCommand, Result<CarBrandDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCarBrandCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CarBrandDto>> Handle(CreateCarBrandCommand request, CancellationToken cancellationToken)
        {
            try
            {
            
                var existingBrand = await _unitOfWork.Repository<CarBrand>()
                    .FindAsync(b => b.Name == request.Dto.Name);

                if (existingBrand is not null)
                    return Result<CarBrandDto>.Failure(EntityError<CarBrand>.Duplicated("Brand already exists"));

                var brand = request.Dto.Adapt<CarBrand>();

    
                await _unitOfWork.Repository<CarBrand>().AddAsync(brand);
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
