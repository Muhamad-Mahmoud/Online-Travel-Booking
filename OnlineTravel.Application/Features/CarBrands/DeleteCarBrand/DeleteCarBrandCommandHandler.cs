using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.CarBrands.DeleteCarBrand
{
	public class DeleteCarBrandCommandHandler : IRequestHandler<DeleteCarBrandCommand, Result<bool>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public DeleteCarBrandCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<bool>> Handle(DeleteCarBrandCommand request, CancellationToken cancellationToken)
		{
			var brand = await _unitOfWork.Repository<CarBrand>().GetByIdAsync(request.Id, cancellationToken);
			if (brand == null)
				return Result<bool>.Failure(Error.NotFound($"Brand with Id {request.Id} not found."));

			_unitOfWork.Repository<CarBrand>().Delete(brand);
			var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

			return result > 0 ? Result<bool>.Success(true) : Result<bool>.Failure(new Error("CarBrand.Delete", "Failed to delete brand.", 500));
		}
	}
}
