using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Admin.System.TriggerSeed;

public record TriggerSeedCommand : IRequest<Result<bool>>;
