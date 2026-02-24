using MediatR;
using OnlineTravel.Application.Features.CarPricingTiers.Common;
using OnlineTravel.Domain.ErrorHandling;
using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineTravel.Application.Features.CarPricingTiers.Update;

public sealed record UpdateCarPricingTierCommand(
    [Required] Guid Id,
    [Required] Guid CarId,
    [Range(0, int.MaxValue)] int FromHours,
    [Range(1, int.MaxValue)] int ToHours,
    [Required] MoneyCommand PricePerHour) : IRequest<Result<UpdateCarPricingTierResponse>>;

