using Mapster;
using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Application.Features.Cars.Specifications;
using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Application.Features.Cars.Shared;

namespace OnlineTravel.Application.Features.Cars.GetCarByIdWithDetails;

public sealed class GetCarByIdWithDetailsQueryHandler : IRequestHandler<GetCarDetailsByIdQuery, Result<CarDetailsResponse>>
{
	private readonly IUnitOfWork _unitOfWork;

	public GetCarByIdWithDetailsQueryHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<CarDetailsResponse>> Handle(GetCarDetailsByIdQuery request, CancellationToken cancellationToken)
	{
		var spec = new CarSpecification()
			.WithId(request.Id)
			.IncludeBrandAndCategory()
			.IncludePricingTiers();

		var car = await _unitOfWork.Repository<Car>().GetEntityWithAsync(spec, cancellationToken);

		if (car == null)
			return Result<CarDetailsResponse>.Failure(Error.NotFound($"Car with Id {request.Id} not found."));

		var dto = car.Adapt<CarDetailsResponse>();
		return Result<CarDetailsResponse>.Success(dto);
	}
}
