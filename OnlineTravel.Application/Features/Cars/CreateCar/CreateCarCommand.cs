using MediatR;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Cars.CreateCar;

public sealed record CreateCarCommand(CreateCarRequest Data) : IRequest<Result<Guid>>;
