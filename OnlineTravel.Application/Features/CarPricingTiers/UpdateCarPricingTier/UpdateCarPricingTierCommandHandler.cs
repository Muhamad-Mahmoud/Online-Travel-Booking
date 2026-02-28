using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Application.Features.Cars.Specifications;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Application.Features.CarPricingTiers.UpdateCarPricingTier;

namespace OnlineTravel.Application.Features.CarPricingTiers.UpdateCarPricingTierCarPricingTier
{
	public class UpdateCarPricingTierCommandHandler : IRequestHandler<UpdateCarPricingTierCommand, Result<UpdateCarPricingTierResponse>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public UpdateCarPricingTierCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<UpdateCarPricingTierResponse>> Handle(UpdateCarPricingTierCommand request, CancellationToken cancellationToken)
		{
			try
			{
				// 1. البحث عن الكيان
				var entity = await _unitOfWork.Repository<CarPricingTier>()
					.GetByIdAsync(request.Id, cancellationToken);
				if (entity is null)
					return EntityError<CarPricingTier>.NotFound();

				// 2. إذا تغير CarId، تحقق من وجود السيارة الجديدة
				if (entity.CarId != request.CarId)
				{
					var car = await _unitOfWork.Repository<Car>()
						.GetByIdAsync(request.CarId, cancellationToken);
					if (car is null)
						return EntityError<Car>.NotFound($"Car with ID {request.CarId} not found");
				}

				// 3. التحقق من صحة النطاق
				if (request.FromHours >= request.ToHours)
					return EntityError<CarPricingTier>.InvalidData("FromHours must be less than ToHours");

				// 4. التحقق من عدم التداخل مع النطاقات الأخرى (باستثناء نفس الكيان)
				var overlapSpec = CarPricingTierSpecification.OverlapSpec(
					request.CarId,
					request.FromHours,
					request.ToHours,
					request.Id); // استبعاد نفس Id

				var overlapping = await _unitOfWork.Repository<CarPricingTier>()
					.GetAllWithSpecAsync(overlapSpec, cancellationToken);

				if (overlapping.Any())
					return EntityError<CarPricingTier>.InvalidData("This time range overlaps with an existing pricing tier");

				// 5. تحديث البيانات
				entity.CarId = request.CarId;
				entity.FromHours = request.FromHours;
				entity.ToHours = request.ToHours;
				entity.PricePerHour = new Money(request.PricePerHour.Amount, request.PricePerHour.Currency);
				entity.UpdatedAt = DateTime.UtcNow;

				_unitOfWork.Repository<CarPricingTier>().Update(entity);
				await _unitOfWork.Complete();

				// 6. Return response
				var response = new UpdateCarPricingTierResponse
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

				return Result<UpdateCarPricingTierResponse>.Success(response);
			}
			catch (Exception ex)
			{
				return EntityError<CarPricingTier>.OperationFailed(ex.Message);
			}
		}
	}
}
