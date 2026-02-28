using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineTravel.Application.Common;
using OnlineTravel.Application.Interfaces.Persistence;

namespace OnlineTravel.Application.Features.Flights.Carrier.GetAllCarriers;

public class GetAllCarriersQueryHandler : IRequestHandler<GetAllCarriersQuery, Result<List<OnlineTravel.Domain.Entities.Flights.Carrier>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllCarriersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<OnlineTravel.Domain.Entities.Flights.Carrier>>> Handle(GetAllCarriersQuery request, CancellationToken cancellationToken)
    {
        var carriers = await _unitOfWork.Repository<OnlineTravel.Domain.Entities.Flights.Carrier>()
            .Query()
            .ToListAsync(cancellationToken);

        return Result<List<OnlineTravel.Domain.Entities.Flights.Carrier>>.Success(carriers);
    }
}
