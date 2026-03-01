using MediatR;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Cars.GetCarById;

public sealed record GetCarByIdQuery(Guid Id) : IRequest<Result<GetCarByIdResponse>>;

