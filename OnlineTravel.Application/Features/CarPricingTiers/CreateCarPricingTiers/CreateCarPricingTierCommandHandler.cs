using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Application.Features.Cars.Specifications;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Application.Features.CarPricingTiers.CreateCarPricingTiers;

namespace OnlineTravel.Application.Features.CarPricingTiers.CreateCarPricingTiersCarPricingTiers
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
			try
			{
				// 1. التحقق من وجود السيارة
				var car = await _unitOfWork.Repository<Car>()
					.GetByIdAsync(request.CarId, cancellationToken);
				if (car is null)
					return EntityError<Car>.NotFound($"Car with ID {request.CarId} not found");

				// 2. التحقق من صحة النطاق
				if (request.FromHours >= request.ToHours)
					return EntityError<CarPricingTier>.InvalidData("FromHours must be less than ToHours");

				// 3. التحقق من عدم التداخل باستخدام Specification
				var overlapSpec = CarPricingTierSpecification.OverlapSpec(
					request.CarId,
					request.FromHours,
					request.ToHours);

				var overlapping = await _unitOfWork.Repository<CarPricingTier>()
					.GetAllWithSpecAsync(overlapSpec, cancellationToken);

				if (overlapping.Any())
					return EntityError<CarPricingTier>.InvalidData("This time range overlaps with an existing pricing tier");

				// 4. إنشاء الكيان
				var money = new Money(request.PricePerHour.Amount, request.PricePerHour.Currency);
				var entity = new CarPricingTier
				{
					CarId = request.CarId,
					FromHours = request.FromHours,
					ToHours = request.ToHours,
					PricePerHour = money
				};

				await _unitOfWork.Repository<CarPricingTier>().AddAsync(entity, cancellationToken);
				await _unitOfWork.Complete();

				// 5. Return response
				var response = new CreateCarPricingTierResponse
				{
					Id = entity.Id,
					CarId = entity.CarId,
					FromHours = entity.FromHours,
					ToHours = entity.ToHours,
					PricePerHour = new MoneyResponse
					{
						Amount = entity.PricePerHour.Amount,
						Currency = entity.PricePerHour.Currency
					}
				};

				return Result<CreateCarPricingTierResponse>.Success(response);
			}
			catch (Exception ex)
			{
				return EntityError<CarPricingTier>.OperationFailed(ex.Message);
			}
		}
	}
}
