using MediatR;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Flight.Carrier.DeleteCarrier
{
    public record DeleteCarrierCommand(Guid Id) : IRequest<Result<bool>>;
}
