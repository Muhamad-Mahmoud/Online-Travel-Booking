using MediatR;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Application.Features.CarPricingTiers.Common;
using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;

namespace OnlineTravel.Application.Features.CarPricingTiers.CreateCarPricingTiers
{
	public class CreateCarPricingTierCommandHandler : IRequestHandler<CreateCarPricingTierCommand, Result<CreateCarPricingTierResponse>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public CreateCarPricingTierCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<CreateCarPricingTierResponse>> Handle(CreateCarPricingTierCommand request, CancellationToken cancellationToken)
		{
			var tier = new CarPricingTier
			{
				CarId = request.CarId,
				FromHours = request.FromHours,
				ToHours = request.ToHours,
				PricePerHour = new Money(request.PricePerHour.Amount, request.PricePerHour.Currency)
			};

			await _unitOfWork.Repository<CarPricingTier>().AddAsync(tier);
			await _unitOfWork.Complete();

			return Result<CreateCarPricingTierResponse>.Success(new CreateCarPricingTierResponse
			{
				Id = tier.Id,
				CarId = tier.CarId,
				FromHours = tier.FromHours,
				ToHours = tier.ToHours,
				PricePerHour = new MoneyResponse { Amount = tier.PricePerHour.Amount, Currency = tier.PricePerHour.Currency }
			});
		}
	}
}

