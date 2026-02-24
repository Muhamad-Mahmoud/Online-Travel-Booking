using Mapster;
using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Cars.CreateCar;

public sealed class CreateCarCommandHandler : IRequestHandler<CreateCarCommand, Result<Guid>>
{
	private readonly IUnitOfWork _unitOfWork;

	public CreateCarCommandHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<Guid>> Handle(CreateCarCommand request, CancellationToken cancellationToken)
	{
		var car = request.Data.Adapt<Car>();

		await _unitOfWork.Repository<Car>().AddAsync(car, cancellationToken);
		var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

		if (result > 0)
			return Result<Guid>.Success(car.Id);

		return Result<Guid>.Failure(new Error("Car.Create", "Failed to create car.", 500));
	}
}
