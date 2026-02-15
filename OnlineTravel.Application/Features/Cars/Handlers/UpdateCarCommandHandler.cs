using Mapster;
using MediatR;
using OnlineTravel.Application.Features.Cars.Commands;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.Entities.Core;
using OnlineTravel.Domain.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Cars.Handlers
{
    public class UpdateCarCommandHandler : IRequestHandler<UpdateCarCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCarCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(UpdateCarCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // البحث عن السيارة
                var car = await _unitOfWork.Repository<Car>()
                    .GetByIdAsync(request.Data.Id, cancellationToken);
                if (car is null)
                    return EntityError<Car>.NotFound();

                // التحقق من العلامة التجارية الجديدة إذا تغيرت
                if (car.BrandId != request.Data.BrandId)
                {
                    var brand = await _unitOfWork.Repository<CarBrand>()
                        .GetByIdAsync(request.Data.BrandId, cancellationToken);
                    if (brand is null)
                        return EntityError<CarBrand>.NotFound($"Brand with ID {request.Data.BrandId} not found");
                }

                // التحقق من التصنيف الجديد إذا تغير
                if (car.CategoryId != request.Data.CategoryId)
                {
                    var category = await _unitOfWork.Repository<Category>()
                        .GetByIdAsync(request.Data.CategoryId, cancellationToken);
                    if (category is null)
                        return EntityError<Category>.NotFound($"Category with ID {request.Data.CategoryId} not found");
                }

                // تحديث البيانات
                request.Data.Adapt(car);
                car.UpdatedAt = DateTime.UtcNow;
                _unitOfWork.Repository<Car>().Update(car);
                await _unitOfWork.Complete();

                return Result<Guid>.Success(car.Id);
            }
            catch (Exception ex)
            {
                return EntityError<Car>.OperationFailed(ex.Message);
            }
        }
    }
}
