using Mapster;
using MediatR;
using OnlineTravel.Application.Features.CarPricingTiers.DTOs;
using OnlineTravel.Application.Features.CarPricingTiers.Queries;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Application.Specifications.Carspec;
using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.CarPricingTiers.Handlers
{
    public class GetAllCarPricingTiersQueryHandler
        : IRequestHandler<GetAllCarPricingTiersQuery, Result<IReadOnlyList<CarPricingTierDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllCarPricingTiersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IReadOnlyList<CarPricingTierDto>>> Handle(
            GetAllCarPricingTiersQuery request,
            CancellationToken cancellationToken)
        {
            // بناء الـ Specification مع الفلترة
            var spec = new CarPricingTierSpecification(request.CarId);

            // جلب العناصر بدون Pagination
            var items = await _unitOfWork.Repository<CarPricingTier>()
                .GetAllWithSpecAsync(spec, cancellationToken);

            var dtos = items.Adapt<IReadOnlyList<CarPricingTierDto>>();

            return Result<IReadOnlyList<CarPricingTierDto>>.Success(dtos);
        }
    }

}
