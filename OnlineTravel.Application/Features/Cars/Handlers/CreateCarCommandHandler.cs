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
    public class CreateCarCommandHandler : IRequestHandler<CreateCarCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCarCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateCarCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // التحقق من وجود العلامة التجارية
                var brand = await _unitOfWork.Repository<CarBrand>()
                    .GetByIdAsync(request.Data.BrandId, cancellationToken);
                if (brand is null)
                    return EntityError<CarBrand>.NotFound($"Brand with ID {request.Data.BrandId} not found");

                // التحقق من وجود التصنيف
                var category = await _unitOfWork.Repository<Category>()
                    .GetByIdAsync(request.Data.CategoryId, cancellationToken);
                if (category is null)
                    return EntityError<Category>.NotFound($"Category with ID {request.Data.CategoryId} not found");

                // التحقق من صحة البيانات (مثلاً SeatsCount > 0)
                if (request.Data.SeatsCount <= 0)
                    return EntityError<Car>.InvalidData("SeatsCount must be greater than zero");

                // إنشاء الكيان
                var car = request.Data.Adapt<Car>();
                await _unitOfWork.Repository<Car>().AddAsync(car, cancellationToken);
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
