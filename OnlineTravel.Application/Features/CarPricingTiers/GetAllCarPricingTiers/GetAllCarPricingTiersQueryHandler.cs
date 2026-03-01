using Mapster;
using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Application.Features.Cars.Specifications;
using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Application.Features.CarPricingTiers.GetAllCarPricingTiers;

namespace OnlineTravel.Application.Features.CarPricingTiers.GetAllCarPricingTiersCarPricingTiers
{
	public class GetAllCarPricingTiersQueryHandler
		: IRequestHandler<GetAllCarPricingTiersQuery, Result<IReadOnlyList<GetAllCarPricingTiersResponse>>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetAllCarPricingTiersQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<IReadOnlyList<GetAllCarPricingTiersResponse>>> Handle(
			GetAllCarPricingTiersQuery request,
			CancellationToken cancellationToken)
		{
			// Build specification with filters
			var spec = new CarPricingTierSpecification(request.CarId);

			// Fetch items without pagination
			var items = await _unitOfWork.Repository<CarPricingTier>()
				.GetAllWithSpecAsync(spec, cancellationToken);

			var responses = items.Adapt<IReadOnlyList<GetAllCarPricingTiersResponse>>();

			return Result<IReadOnlyList<GetAllCarPricingTiersResponse>>.Success(responses);
		}
	}
}
