using Mapster;
using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.CarPricingTiers.GetById
{
	public class GetCarPricingTierByIdQueryHandler : IRequestHandler<GetCarPricingTierByIdQuery, Result<GetCarPricingTierByIdResponse>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetCarPricingTierByIdQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<GetCarPricingTierByIdResponse>> Handle(GetCarPricingTierByIdQuery request, CancellationToken cancellationToken)
		{
			var entity = await _unitOfWork.Repository<CarPricingTier>()
				.GetByIdAsync(request.Id, cancellationToken);

			if (entity is null)
				return EntityError<CarPricingTier>.NotFound();

			var response = entity.Adapt<GetCarPricingTierByIdResponse>();
			return Result<GetCarPricingTierByIdResponse>.Success(response);
		}
	}
}
