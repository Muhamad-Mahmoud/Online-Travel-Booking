using Domain.ErrorHandling;
using MediatR;
using OnlineTravel.Application.Features.CarBrands.Commands;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Cars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.CarBrands.Handlers
{
    public class DeleteCarBrandCommandHandler : IRequestHandler<DeleteCarBrandCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCarBrandCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(DeleteCarBrandCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var brand = await _unitOfWork.Repository<CarBrand>().GetByIdAsync(request.Id);
                if (brand is null)
                    return Result<bool>.Failure(EntityError<CarBrand>.NotFound());

              
                _unitOfWork.Repository<CarBrand>().Delete(brand);
                await _unitOfWork.Complete();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(EntityError<CarBrand>.OperationFailed(ex.Message));
            }
        }
    }
}
