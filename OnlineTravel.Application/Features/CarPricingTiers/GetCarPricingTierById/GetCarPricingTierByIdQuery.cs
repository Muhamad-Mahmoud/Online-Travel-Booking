using MediatR;
using OnlineTravel.Domain.ErrorHandling;
using System;

namespace OnlineTravel.Application.Features.CarPricingTiers.GetById;

public sealed record GetCarPricingTierByIdQuery(Guid Id) : IRequest<Result<GetCarPricingTierByIdResponse>>;
