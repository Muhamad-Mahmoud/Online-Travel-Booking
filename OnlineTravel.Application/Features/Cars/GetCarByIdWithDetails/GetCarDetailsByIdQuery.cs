using MediatR;
using OnlineTravel.Application.Features.Cars.Shared;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Cars.GetCarByIdWithDetails;

public sealed record GetCarDetailsByIdQuery(Guid Id) : IRequest<Result<CarDetailsResponse>>;
