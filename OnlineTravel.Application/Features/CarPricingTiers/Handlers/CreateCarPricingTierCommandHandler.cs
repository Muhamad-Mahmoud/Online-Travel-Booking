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

    public class CreateCarPricingTierCommandHandler : IRequestHandler<CreateCarPricingTierCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCarPricingTierCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateCarPricingTierCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // 1. التحقق من وجود السيارة
                var car = await _unitOfWork.Repository<Car>()
                    .GetByIdAsync(request.Data.CarId, cancellationToken);
                if (car is null)
                    return EntityError<Car>.NotFound($"Car with ID {request.Data.CarId} not found");

                // 2. التحقق من صحة النطاق
                if (request.Data.FromHours >= request.Data.ToHours)
                    return EntityError<CarPricingTier>.InvalidData("FromHours must be less than ToHours");

                // 3. التحقق من عدم التداخل باستخدام Specification
                var overlapSpec = CarPricingTierSpecification.OverlapSpec(
                    request.Data.CarId,
                    request.Data.FromHours,
                    request.Data.ToHours);

                var overlapping = await _unitOfWork.Repository<CarPricingTier>()
                    .GetAllWithSpecAsync(overlapSpec, cancellationToken);

                if (overlapping.Any())
                    return EntityError<CarPricingTier>.InvalidData("This time range overlaps with an existing pricing tier");

                // 4. إنشاء الكيان
                var entity = request.Data.Adapt<CarPricingTier>();
                await _unitOfWork.Repository<CarPricingTier>().AddAsync(entity, cancellationToken);
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
