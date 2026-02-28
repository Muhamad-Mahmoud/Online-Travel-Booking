using MediatR;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Flights.Airport.DeleteAirport
{
    public record DeleteAirportCommand(Guid Id) : IRequest<Result<bool>>;
}
