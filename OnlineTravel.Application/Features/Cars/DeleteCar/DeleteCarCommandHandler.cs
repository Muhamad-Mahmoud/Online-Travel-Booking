using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Cars.DeleteCar;

public sealed class DeleteCarCommandHandler : IRequestHandler<DeleteCarCommand, Result<bool>>
{
	private readonly IUnitOfWork _unitOfWork;

	public DeleteCarCommandHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<bool>> Handle(DeleteCarCommand request, CancellationToken cancellationToken)
	{
		var existingCar = await _unitOfWork.Repository<Car>().GetByIdAsync(request.Id, cancellationToken);
		if (existingCar == null)
			return Result<bool>.Failure(Error.NotFound($"Car with Id {request.Id} not found."));

		_unitOfWork.Repository<Car>().Delete(existingCar);
		var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

		return result > 0 ? Result<bool>.Success(true) : Result<bool>.Failure(new Error("Car.Delete", "Failed to delete car.", 500));
	}
}
