using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Domain.Entities.Flights;

namespace OnlineTravel.Application.Features.Flights.Flights.GetFlightById;

public record GetFlightByIdQuery(Guid Id) : IRequest<Result<OnlineTravel.Domain.Entities.Flights.Flight>>;
