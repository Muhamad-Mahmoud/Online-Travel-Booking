using MediatR;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Flights.Carriers.DeleteCarrier
{
    public record DeleteCarrierCommand(Guid Id) : IRequest<OnlineTravel.Application.Common.Result<bool>>;
}
