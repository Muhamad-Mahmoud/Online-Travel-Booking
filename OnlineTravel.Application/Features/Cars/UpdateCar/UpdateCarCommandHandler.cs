using Mapster;
using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Cars.UpdateCar;

public sealed class UpdateCarCommandHandler : IRequestHandler<UpdateCarCommand, Result<bool>>
{
	private readonly IUnitOfWork _unitOfWork;

	public UpdateCarCommandHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<bool>> Handle(UpdateCarCommand request, CancellationToken cancellationToken)
	{
		var existingCar = await _unitOfWork.Repository<Car>().GetByIdAsync(request.Data.Id, cancellationToken);
		if (existingCar == null)
			return Result<bool>.Failure(Error.NotFound($"Car with Id {request.Data.Id} not found."));

		request.Data.Adapt(existingCar);

		_unitOfWork.Repository<Car>().Update(existingCar);
		var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

		return result > 0 ? Result<bool>.Success(true) : Result<bool>.Failure(new Error("Car.Update", "Failed to update car.", 500));
	}
}
