using MediatR;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Cars.UpdateCar;

public sealed record UpdateCarCommand(UpdateCarRequest Data) : IRequest<Result<bool>>;
