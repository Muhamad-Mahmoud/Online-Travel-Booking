using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Domain.Exceptions;

namespace OnlineTravel.Application.Features.Flights.Flights.GetFlights;

public record GetFlightsQuery(int PageIndex = 1, int PageSize = 10, string? Search = null, string? Status = null)
	: IRequest<Result<PaginatedResult<OnlineTravel.Domain.Entities.Flights.Flight>>>;
