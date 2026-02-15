using Mapster;
using MediatR;
using OnlineTravel.Application.Features.CarPricingTiers.Commands;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Application.Specifications.Carspec;
using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.CarPricingTiers.Handlers
{
    public class UpdateCarPricingTierCommandHandler : IRequestHandler<UpdateCarPricingTierCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCarPricingTierCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(UpdateCarPricingTierCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // 1. البحث عن الكيان
                var entity = await _unitOfWork.Repository<CarPricingTier>()
                    .GetByIdAsync(request.Data.Id, cancellationToken);
                if (entity is null)
                    return EntityError<CarPricingTier>.NotFound();

                // 2. إذا تغير CarId، تحقق من وجود السيارة الجديدة
                if (entity.CarId != request.Data.CarId)
                {
                    var car = await _unitOfWork.Repository<Car>()
                        .GetByIdAsync(request.Data.CarId, cancellationToken);
                    if (car is null)
                        return EntityError<Car>.NotFound($"Car with ID {request.Data.CarId} not found");
                }

                // 3. التحقق من صحة النطاق
                if (request.Data.FromHours >= request.Data.ToHours)
                    return EntityError<CarPricingTier>.InvalidData("FromHours must be less than ToHours");

                // 4. التحقق من عدم التداخل مع النطاقات الأخرى (باستثناء نفس الكيان)
                var overlapSpec = CarPricingTierSpecification.OverlapSpec(
                    request.Data.CarId,
                    request.Data.FromHours,
                    request.Data.ToHours,
                    request.Data.Id); // استبعاد نفس Id

                var overlapping = await _unitOfWork.Repository<CarPricingTier>()
                    .GetAllWithSpecAsync(overlapSpec, cancellationToken);

                if (overlapping.Any())
                    return EntityError<CarPricingTier>.InvalidData("This time range overlaps with an existing pricing tier");

                // 5. تحديث البيانات
                request.Data.Adapt(entity);
                entity.UpdatedAt = DateTime.UtcNow;
                _unitOfWork.Repository<CarPricingTier>().Update(entity);
                await _unitOfWork.Complete();

                return Result<Guid>.Success(entity.Id);
            }
            catch (Exception ex)
            {
                return EntityError<CarPricingTier>.OperationFailed(ex.Message);
            }
        }
    }
}
