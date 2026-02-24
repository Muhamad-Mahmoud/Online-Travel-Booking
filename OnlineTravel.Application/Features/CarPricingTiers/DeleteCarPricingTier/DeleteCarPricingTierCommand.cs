using MediatR;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.CarPricingTiers.Delete;

public sealed record DeleteCarPricingTierCommand(Guid Id) : IRequest<Result>;
