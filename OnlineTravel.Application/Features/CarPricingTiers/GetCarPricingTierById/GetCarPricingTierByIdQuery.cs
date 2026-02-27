using MediatR;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.CarPricingTiers.GetById;

public sealed record GetCarPricingTierByIdQuery(Guid Id) : IRequest<Result<GetCarPricingTierByIdResponse>>;
