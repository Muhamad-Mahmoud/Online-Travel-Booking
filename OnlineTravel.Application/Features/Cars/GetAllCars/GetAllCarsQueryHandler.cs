using Mapster;
using MediatR;
using OnlineTravel.Application.Features.Cars.GetCarById;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Application.Specifications.Carspec;
using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Domain.Exceptions;

namespace OnlineTravel.Application.Features.Cars.GetAllCars;

public sealed class GetAllCarsQueryHandler : IRequestHandler<GetAllCarsQuery, Result<PaginatedResult<CarDto>>>
{
	private readonly IUnitOfWork _unitOfWork;

	public GetAllCarsQueryHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<PaginatedResult<CarDto>>> Handle(GetAllCarsQuery request, CancellationToken cancellationToken)
	{
		var spec = new CarSpecification(request.BrandId)
			.IncludeBrandAndCategory();

		if (request.CategoryId.HasValue)
			spec.WithCategory(request.CategoryId.Value);
		if (request.CarType.HasValue)
			spec.WithCarType(request.CarType.Value);

		spec.ApplyPagination((request.PageIndex - 1) * request.PageSize, request.PageSize);

		var items = await _unitOfWork.Repository<Car>().GetAllWithSpecAsync(spec, cancellationToken);
		
		var countSpec = new CarSpecification(request.BrandId);
		if (request.CategoryId.HasValue)
			countSpec.WithCategory(request.CategoryId.Value);
		if (request.CarType.HasValue)
			countSpec.WithCarType(request.CarType.Value);

		var totalCount = await _unitOfWork.Repository<Car>().GetCountAsync(countSpec, cancellationToken);

		var dtos = items.Adapt<IReadOnlyList<CarDto>>();
		var paginated = new PaginatedResult<CarDto>(request.PageIndex, request.PageSize, totalCount, dtos);

		return Result<PaginatedResult<CarDto>>.Success(paginated);
	}
}
