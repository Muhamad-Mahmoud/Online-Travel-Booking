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
    public class HotelGalleryConfiguration : IEntityTypeConfiguration<HotelGallery>
    {
        public void Configure(EntityTypeBuilder<HotelGallery> builder)
        {
            builder.ToTable("HotelGallery");

            builder.HasKey(g => g.Id);

            builder.Property(g => g.HotelId)
                .IsRequired();

            builder.Property(g => g.Url)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(g => g.Alt)
                .HasMaxLength(200);
        }
    }

}
