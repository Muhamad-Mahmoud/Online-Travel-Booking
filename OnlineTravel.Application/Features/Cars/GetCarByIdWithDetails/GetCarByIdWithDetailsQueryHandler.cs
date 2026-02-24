using Mapster;
using MediatR;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Application.Specifications.Carspec;
using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Cars.GetCarByIdWithDetails;

public sealed class GetCarByIdWithDetailsQueryHandler : IRequestHandler<GetCarDetailsByIdQuery, Result<CarDetailsDto>>
{
	private readonly IUnitOfWork _unitOfWork;

	public GetCarByIdWithDetailsQueryHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<CarDetailsDto>> Handle(GetCarDetailsByIdQuery request, CancellationToken cancellationToken)
	{
		var spec = new CarSpecification(request.Id)
			.IncludeBrandAndCategory()
			.IncludePricingTiers();

		var car = await _unitOfWork.Repository<Car>().GetEntityWithAsync(spec, cancellationToken);

		if (car == null)
			return Result<CarDetailsDto>.Failure(Error.NotFound($"Car with Id {request.Id} not found."));

		var dto = car.Adapt<CarDetailsDto>();
		return Result<CarDetailsDto>.Success(dto);
	}
}
