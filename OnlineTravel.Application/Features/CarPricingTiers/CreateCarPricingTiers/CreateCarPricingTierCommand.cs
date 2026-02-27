using System.ComponentModel.DataAnnotations;
using MediatR;
using OnlineTravel.Application.Features.CarPricingTiers.Common;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.CarPricingTiers.CreateCarPricingTiers;

public sealed record CreateCarPricingTierCommand(
	[Required] Guid CarId,
	[Range(0, int.MaxValue)] int FromHours,
	[Range(1, int.MaxValue)] int ToHours,
	[Required] MoneyCommand PricePerHour) : IRequest<Result<CreateCarPricingTierResponse>>;
