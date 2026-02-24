using Mapster;
using MediatR;
using OnlineTravel.Application.Features.CarBrands.Shared.DTOs;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Cars;
using OnlineTravel.Domain.ErrorHandling;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.CarBrands.GetCarBrandById
{
    public class GetCarBrandByIdQueryHandler : IRequestHandler<GetCarBrandByIdQuery, Result<CarBrandDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCarBrandByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CarBrandDto>> Handle(GetCarBrandByIdQuery request, CancellationToken cancellationToken)
        {
            var brand = await _unitOfWork.Repository<CarBrand>().GetByIdAsync(request.Id, cancellationToken);

            if (brand == null)
                return Result<CarBrandDto>.Failure(Error.NotFound($"Brand with Id {request.Id} not found."));

            var dto = brand.Adapt<CarBrandDto>();
            return Result<CarBrandDto>.Success(dto);
        }
    }
}
