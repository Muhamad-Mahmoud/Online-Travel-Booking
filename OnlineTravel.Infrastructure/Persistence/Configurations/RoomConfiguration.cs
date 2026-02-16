using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Hotels;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.RoomNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(r => r.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(r => r.Capacity)
                .IsRequired();

            builder.Property(r => r.BedCount)
                .IsRequired();

            // Money owned type
            builder.OwnsOne(r => r.BasePricePerNight, money =>
            {
                money.Property(m => m.Amount)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
                money.Property(m => m.Currency)
                    .IsRequired()
                    .HasMaxLength(3);
            });

            // Photos collection
            builder.OwnsMany(r => r.Photos, photo =>
            {
                photo.Property(p => p.Value)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            // Unique constraint
            builder.HasIndex(r => new { r.HotelId, r.RoomNumber })
                .IsUnique();

            // Relationships
            builder.HasMany(r => r.SeasonalPrices)
                .WithOne(sp => sp.Room)
                .HasForeignKey(sp => sp.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(r => r.RoomAvailabilities)
                .WithOne(ra => ra.Room)
                .HasForeignKey(ra => ra.RoomId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
