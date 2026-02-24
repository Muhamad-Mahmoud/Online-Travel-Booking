using MediatR;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Cars.DeleteCar;

public sealed record DeleteCarCommand(Guid Id) : IRequest<Result<bool>>;
