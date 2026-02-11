using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravel.Domain.Entities.Tours;

namespace OnlineTravel.Infrastructure.Persistence.Configurations;

public class TourPriceTierConfiguration : IEntityTypeConfiguration<TourPriceTier>
{
    public void Configure(EntityTypeBuilder<TourPriceTier> builder)
    {
        builder.ToTable("TourPriceTiers", "tours");

        builder.OwnsOne(e => e.Price, m =>
        {
            m.Property(p => p.Amount).HasColumnName("Price");
            m.Property(p => p.Currency).HasColumnName("Currency").HasDefaultValue("USD");
        });

        builder.Property(e => e.Name).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Description).HasMaxLength(1000);

        builder.HasMany(e => e.Schedules)
            .WithOne(s => s.PriceTier)
            .HasForeignKey(s => s.PriceTierId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}




