using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Domain.Entities.Flights;

namespace OnlineTravel.Application.Features.Flight.Carrier.GetAllCarriers;

public record GetAllCarriersQuery() : IRequest<Result<List<OnlineTravel.Domain.Entities.Flights.Carrier>>>;
