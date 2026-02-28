using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Application.Features.Cars.Shared;
using OnlineTravel.Application.Interfaces.Persistence;

namespace OnlineTravel.Application.Features.Cars.GetAllCarsSummary
{
	public class GetAllCarsSummaryQueryHandler : IRequestHandler<GetAllCarsSummaryQuery, Result<PagedResult<CarSummaryResponse>>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetAllCarsSummaryQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<PagedResult<CarSummaryResponse>>> Handle(GetAllCarsSummaryQuery request, CancellationToken cancellationToken)
		{
			// Simplification for recovery
			var cars = await _unitOfWork.Repository<OnlineTravel.Domain.Entities.Cars.Car>().GetAllAsync();
			
			var data = cars.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).Select(c => new CarSummaryResponse
			{
				Id = c.Id,
				Make = c.Make,
				Model = c.Model,
				CarType = c.CarType,
				SeatsCount = c.SeatsCount,
				FuelType = c.FuelType,
				Transmission = c.Transmission,
				PricePerHour = c.PricingTiers.OrderBy(t => t.FromHours).FirstOrDefault()?.PricePerHour.Amount ?? 0
			}).ToList();

			var result = new PagedResult<CarSummaryResponse>(data, cars.Count, request.PageNumber, request.PageSize);
			return Result<PagedResult<CarSummaryResponse>>.Success(result);
		}

	}
}

