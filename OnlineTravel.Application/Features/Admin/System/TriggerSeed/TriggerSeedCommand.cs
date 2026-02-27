using MediatR;
using OnlineTravel.Application.Common;

namespace OnlineTravel.Application.Features.Admin.System.TriggerSeed;

public record TriggerSeedCommand : IRequest<Result<bool>>;
