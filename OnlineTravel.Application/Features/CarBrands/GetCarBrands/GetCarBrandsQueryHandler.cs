using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineTravel.Application.Features.CarBrands.Shared;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.CarBrands.GetCarBrands;

public sealed class GetCarBrandsQueryHandler : IRequestHandler<GetCarBrandsQuery, Result<List<CarBrandResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCarBrandsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<CarBrandResponse>>> Handle(GetCarBrandsQuery request, CancellationToken cancellationToken)
    {
        var brands = await _unitOfWork.Repository<OnlineTravel.Domain.Entities.Cars.CarBrand>()
            .Query()
            .ProjectToType<CarBrandResponse>()
            .ToListAsync(cancellationToken);

        return Result<List<CarBrandResponse>>.Success(brands);
    }
}
