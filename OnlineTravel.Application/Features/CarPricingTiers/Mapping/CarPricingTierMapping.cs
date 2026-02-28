using Mapster;
using OnlineTravel.Application.Features.CarPricingTiers.Common;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;

namespace OnlineTravel.Application.Features.CarPricingTiers.Mapping
{
	public class CarPricingTierMapping : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			// Money <-> MoneyCommand mappings
			// Mapster will automatically handle direct property mappings
			// Custom mapping for Money ValueObject  
			config.NewConfig<Money, MoneyCommand>()
				.Map(dest => dest.Amount, src => src.Amount)
				.Map(dest => dest.Currency, src => src.Currency);

			config.NewConfig<MoneyCommand, Money>()
				.ConstructUsing(src => new Money(src.Amount, src.Currency));
		}
	}
}
