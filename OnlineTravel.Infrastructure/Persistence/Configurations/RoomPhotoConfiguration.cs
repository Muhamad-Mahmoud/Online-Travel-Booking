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
    public class RoomPhotoConfiguration : IEntityTypeConfiguration<RoomPhoto>
    {
        public void Configure(EntityTypeBuilder<RoomPhoto> builder)
        {
            builder.ToTable("RoomPhotos");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.RoomId)
                .IsRequired();

            builder.Property(p => p.Url)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(p => p.Alt)
                .HasMaxLength(200);
        }
    }

}
