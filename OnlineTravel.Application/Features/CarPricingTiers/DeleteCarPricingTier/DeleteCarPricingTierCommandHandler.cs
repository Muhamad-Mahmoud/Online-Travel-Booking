using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.CarPricingTiers.Delete
{
	public class DeleteCarPricingTierCommandHandler : IRequestHandler<DeleteCarPricingTierCommand, Result>
	{
		private readonly IUnitOfWork _unitOfWork;

		public DeleteCarPricingTierCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result> Handle(DeleteCarPricingTierCommand request, CancellationToken cancellationToken)
		{
			try
			{
				var entity = await _unitOfWork.Repository<CarPricingTier>()
					.GetByIdAsync(request.Id, cancellationToken);
				if (entity is null)
					return Result.Failure(EntityError<CarPricingTier>.NotFound());

				_unitOfWork.Repository<CarPricingTier>().Delete(entity);
				await _unitOfWork.Complete();

				return Result.Success();
			}
			catch (Exception ex)
			{
				return Result.Failure(EntityError<CarPricingTier>.OperationFailed(ex.Message));
			}
		}
	}
}
