using Mapster;
using MediatR;
using OnlineTravel.Application.Features.CarPricingTiers.DTOs;
using OnlineTravel.Application.Features.CarPricingTiers.Queries;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.CarPricingTiers.Handlers
{
    public class GetCarPricingTierByIdQueryHandler : IRequestHandler<GetCarPricingTierByIdQuery, Result<CarPricingTierDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCarPricingTierByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CarPricingTierDto>> Handle(GetCarPricingTierByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<CarPricingTier>()
                .GetByIdAsync(request.Id, cancellationToken);

            if (entity is null)
                return EntityError<CarPricingTier>.NotFound();

            var dto = entity.Adapt<CarPricingTierDto>();
            return Result<CarPricingTierDto>.Success(dto);
        }
    }
}
