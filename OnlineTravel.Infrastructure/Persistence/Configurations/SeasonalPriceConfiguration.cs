using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravel.Domain.Entities.Hotels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Infrastructure.Persistence.Configurations
{
    public class SeasonalPriceConfiguration : IEntityTypeConfiguration<SeasonalPrice>
    {
        public void Configure(EntityTypeBuilder<SeasonalPrice> builder)
        {
            builder.HasKey(sp => sp.Id);

            builder.OwnsOne(sp => sp.DateRange, dr =>
            {
                dr.Property(d => d.Start).IsRequired();
                dr.Property(d => d.End).IsRequired();
            });

            builder.OwnsOne(sp => sp.PricePerNight, money =>
            {
                money.Property(m => m.Amount)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
                money.Property(m => m.Currency)
                    .IsRequired()
                    .HasMaxLength(3);
            });
        }
    }

}
