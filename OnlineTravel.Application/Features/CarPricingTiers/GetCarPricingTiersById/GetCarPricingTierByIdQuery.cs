using MediatR;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.CarPricingTiers.GetCarPricingTiersById;

public sealed record GetCarPricingTierByIdQuery(Guid Id) : IRequest<Result<GetCarPricingTierByIdResponse>>;
