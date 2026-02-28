using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineTravel.Application.Common;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Exceptions;

namespace OnlineTravel.Application.Features.Flights.Flights.GetFlights;

public class GetFlightsQueryHandler : IRequestHandler<GetFlightsQuery, Result<PaginatedResult<OnlineTravel.Domain.Entities.Flights.Flight>>>
{
	private readonly IUnitOfWork _unitOfWork;

	public GetFlightsQueryHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<PaginatedResult<OnlineTravel.Domain.Entities.Flights.Flight>>> Handle(GetFlightsQuery request, CancellationToken cancellationToken)
	{
		var repository = _unitOfWork.Repository<OnlineTravel.Domain.Entities.Flights.Flight>();
		var query = repository.Query()
			.Include(f => f.OriginAirport)
			.Include(f => f.DestinationAirport)
			.Include(f => f.Carrier)
			.Include(f => f.Seats)
			.AsNoTracking();

		if (!string.IsNullOrWhiteSpace(request.Search))
		{
			query = query.Where(f => f.FlightNumber.Value.Contains(request.Search));
		}

		if (!string.IsNullOrWhiteSpace(request.Status) && Enum.TryParse<OnlineTravel.Domain.Enums.FlightStatus>(request.Status, out var status))
		{
			query = query.Where(f => f.Status == status);
		}

		var totalCount = await query.CountAsync(cancellationToken);
		var items = await query
			.OrderByDescending(f => f.Schedule.Start)
			.Skip((request.PageIndex - 1) * request.PageSize)
			.Take(request.PageSize)
			.ToListAsync(cancellationToken);

		var result = new PaginatedResult<OnlineTravel.Domain.Entities.Flights.Flight>(request.PageIndex, request.PageSize, totalCount, items);
		return Result<PaginatedResult<OnlineTravel.Domain.Entities.Flights.Flight>>.Success(result);
	}
}
