using MediatR;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Flight.Airport.DeleteAirport
{
    public record DeleteAirportCommand(Guid Id) : IRequest<Result<bool>>;
}
