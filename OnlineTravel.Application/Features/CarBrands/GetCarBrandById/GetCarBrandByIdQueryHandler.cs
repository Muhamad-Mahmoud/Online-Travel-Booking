using Mapster;
using MediatR;
using OnlineTravel.Application.Features.CarBrands.Shared;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.CarBrands.GetCarBrandById
{
	public class GetCarBrandByIdQueryHandler : IRequestHandler<GetCarBrandByIdQuery, Result<CarBrandResponse>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetCarBrandByIdQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<CarBrandResponse>> Handle(GetCarBrandByIdQuery request, CancellationToken cancellationToken)
		{
			var brand = await _unitOfWork.Repository<CarBrand>().GetByIdAsync(request.Id, cancellationToken);

			if (brand == null)
				return Result<CarBrandResponse>.Failure(Error.NotFound($"Brand with Id {request.Id} not found."));

			var dto = brand.Adapt<CarBrandResponse>();
			return Result<CarBrandResponse>.Success(dto);
		}
	}
}
