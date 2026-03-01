using Mapster;
using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Application.Features.Cars.Specifications;
using OnlineTravel.Domain.Entities.Cars;

namespace OnlineTravel.Application.Features.Cars.GetCarById;

public sealed class GetCarByIdQueryHandler : IRequestHandler<GetCarByIdQuery, Result<GetCarByIdResponse>>
{
	private readonly IUnitOfWork _unitOfWork;

	public GetCarByIdQueryHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<GetCarByIdResponse>> Handle(GetCarByIdQuery request, CancellationToken cancellationToken)
	{
		var spec = new CarSpecification()
			.WithId(request.Id)
			.IncludeBrandAndCategory();

		var car = await _unitOfWork.Repository<Car>().GetEntityWithAsync(spec, cancellationToken);

		if (car == null)
			return Result<GetCarByIdResponse>.Failure($"Car with Id {request.Id} not found.");

		var dto = car.Adapt<GetCarByIdResponse>();
		return Result<GetCarByIdResponse>.Success(dto);
	}
}

