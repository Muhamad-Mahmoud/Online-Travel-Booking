namespace OnlineTravel.Application.Features.CarPricingTiers.Common;

public record MoneyCommand(decimal Amount, string Currency = "USD");
