using Mapster;
using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Application.Specifications.Carspec;
using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.CarPricingTiers.GetAll
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
			// بناء الـ Specification مع الفلترة
			var spec = new CarPricingTierSpecification(request.CarId);

			// جلب العناصر بدون Pagination
			var items = await _unitOfWork.Repository<CarPricingTier>()
				.GetAllWithSpecAsync(spec, cancellationToken);

			var responses = items.Adapt<IReadOnlyList<GetAllCarPricingTiersResponse>>();

			return Result<IReadOnlyList<GetAllCarPricingTiersResponse>>.Success(responses);
		}
	}
}
