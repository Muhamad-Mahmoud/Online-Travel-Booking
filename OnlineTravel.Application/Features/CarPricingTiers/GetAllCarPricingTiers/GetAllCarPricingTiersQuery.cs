using MediatR;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.CarPricingTiers.GetAllCarPricingTiers;

public sealed record GetAllCarPricingTiersQuery(Guid? CarId = null) : IRequest<Result<IReadOnlyList<GetAllCarPricingTiersResponse>>>;
