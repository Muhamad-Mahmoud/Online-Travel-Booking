using Mapster;
using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Application.Features.Cars.Specifications;
using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Cars.GetCarById;

public sealed class GetCarByIdQueryHandler : IRequestHandler<GetCarByIdQuery, Result<CarDto>>
{
	private readonly IUnitOfWork _unitOfWork;

	public GetCarByIdQueryHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<CarDto>> Handle(GetCarByIdQuery request, CancellationToken cancellationToken)
	{
		var spec = new CarSpecification()
			.WithId(request.Id)
			.IncludeBrandAndCategory();

		var car = await _unitOfWork.Repository<Car>().GetEntityWithAsync(spec, cancellationToken);

		if (car == null)
			return Result<CarDto>.Failure(Error.NotFound($"Car with Id {request.Id} not found."));

		var dto = car.Adapt<CarDto>();
		return Result<CarDto>.Success(dto);
	}
}
