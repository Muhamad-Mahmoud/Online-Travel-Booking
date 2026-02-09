using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravel.Domain.Entities.Cars;

namespace OnlineTravel.Infrastructure.Persistence.Configurations;

public class CarExtraConfiguration : IEntityTypeConfiguration<CarExtra>
{
    public void Configure(EntityTypeBuilder<CarExtra> builder)
    {
        builder.ToTable("CarExtras", "cars");

        builder.OwnsOne(e => e.PricePerDay, m =>
        {
            m.Property(p => p.Amount).HasColumnName("PricePerDay");
            m.Property(p => p.Currency).HasColumnName("Currency");
        });

        builder.OwnsOne(e => e.PricePerRental, m =>
        {
            m.Property(p => p.Amount).HasColumnName("PricePerRental");
            m.Property(p => p.Currency).HasColumnName("Currency");
        });
    }

}




