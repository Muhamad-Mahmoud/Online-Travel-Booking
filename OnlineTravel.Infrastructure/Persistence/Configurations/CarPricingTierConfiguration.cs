using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravel.Domain.Entities.Cars;

namespace OnlineTravel.Infrastructure.Persistence.Configurations;

public class CarPricingTierConfiguration : IEntityTypeConfiguration<CarPricingTier>
{
    public void Configure(EntityTypeBuilder<CarPricingTier> builder)
    {
        builder.ToTable("CarPricingTiers", "cars");

        builder.OwnsOne(e => e.PricePerHour, m =>
        {
            m.Property(p => p.Amount).HasColumnName("PricePerHour");
            m.Property(p => p.Currency).HasColumnName("Currency");
        });
    }

}




