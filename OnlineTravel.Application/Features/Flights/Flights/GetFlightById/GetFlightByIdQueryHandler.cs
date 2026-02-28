using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineTravel.Application.Common;
using OnlineTravel.Application.Interfaces.Persistence;

namespace OnlineTravel.Application.Features.Flights.Flights.GetFlightById;

public class GetFlightByIdQueryHandler : IRequestHandler<GetFlightByIdQuery, Result<OnlineTravel.Domain.Entities.Flights.Flight>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetFlightByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<OnlineTravel.Domain.Entities.Flights.Flight>> Handle(GetFlightByIdQuery request, CancellationToken cancellationToken)
    {
        var flight = await _unitOfWork.Repository<OnlineTravel.Domain.Entities.Flights.Flight>()
            .Query()
            .Include(f => f.OriginAirport)
            .Include(f => f.DestinationAirport)
            .Include(f => f.Carrier)
            .Include(f => f.Seats)
            .Include(f => f.Fares)
            .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);

        if (flight == null)
            return Result<OnlineTravel.Domain.Entities.Flights.Flight>.Failure("Flight not found");

        return Result<OnlineTravel.Domain.Entities.Flights.Flight>.Success(flight);
    }
}
