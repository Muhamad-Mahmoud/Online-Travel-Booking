using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravel.Domain.Entities.Bookings;

namespace OnlineTravel.Infrastructure.Persistence.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings", "bookings");

        builder.OwnsOne(e => e.BookingReference, br =>
        {
            br.Property(p => p.Value).HasColumnName("BookingReference");
        });

        builder.HasIndex(e => new { e.UserId, e.Status });
        builder.HasIndex(e => e.BookingDate);

        builder.OwnsOne(e => e.TotalPrice, m =>
        {
            m.Property(p => p.Amount).HasColumnName("TotalPrice");
            m.Property(p => p.Currency).HasColumnName("Currency");
        });

        builder.HasMany(e => e.Details)
            .WithOne(d => d.Booking)
            .HasForeignKey(d => d.BookingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}




