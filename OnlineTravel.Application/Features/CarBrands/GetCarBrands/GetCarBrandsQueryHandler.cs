using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineTravel.Application.Features.CarBrands.Shared;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.ErrorHandling;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.CarBrands.GetCarBrands;

public sealed class GetCarBrandsQueryHandler : IRequestHandler<GetCarBrandsQuery, Result<List<CarBrandResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCarBrandsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<List<CarBrandResponse>>> Handle(GetCarBrandsQuery request, CancellationToken cancellationToken)
    {
        var brands = await _unitOfWork.Repository<OnlineTravel.Domain.Entities.Cars.CarBrand>()
            .Query()
            .ProjectTo<CarBrandResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return Result<List<CarBrandResponse>>.Success(brands);
    }
}
