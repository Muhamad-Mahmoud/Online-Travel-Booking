using Mapster;
using OnlineTravel.Application.Features.CarPricingTiers.DTOs;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Cars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Mapping
{
    public class CarPricingTierMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // Money <-> MoneyDto
            config.NewConfig<Money, MoneyDto>();
            config.NewConfig<MoneyDto, Money>()
                .ConstructUsing(src => new Money(src.Amount, src.Currency));

            // CarPricingTier -> CarPricingTierDto
            config.NewConfig<CarPricingTier, CarPricingTierDto>();

            // CreateCarPricingTierRequest -> CarPricingTier
            config.NewConfig<CreateCarPricingTierRequest, CarPricingTier>()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.CreatedAt)
                .Ignore(dest => dest.UpdatedAt)
                .Ignore(dest => dest.Car);

            // UpdateCarPricingTierRequest -> CarPricingTier
            config.NewConfig<UpdateCarPricingTierRequest, CarPricingTier>()
                .Ignore(dest => dest.CreatedAt)
                .Ignore(dest => dest.UpdatedAt)
                .Ignore(dest => dest.Car);
        }
    }
}
