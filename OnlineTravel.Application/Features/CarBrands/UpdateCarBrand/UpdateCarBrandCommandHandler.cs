using Mapster;
using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.CarBrands.UpdateCarBrand
{
	public class UpdateCarBrandCommandHandler : IRequestHandler<UpdateCarBrandCommand, Result<bool>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public UpdateCarBrandCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<bool>> Handle(UpdateCarBrandCommand request, CancellationToken cancellationToken)
		{
			var brand = await _unitOfWork.Repository<CarBrand>().GetByIdAsync(request.Data.Id, cancellationToken);
			if (brand == null)
				return Result<bool>.Failure(Error.NotFound($"Brand with Id {request.Data.Id} not found."));

			request.Data.Adapt(brand);

			_unitOfWork.Repository<CarBrand>().Update(brand);
			var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

			return result > 0 ? Result<bool>.Success(true) : Result<bool>.Failure(new Error("CarBrand.Update", "Failed to update brand.", 500));
		}
	}
}
