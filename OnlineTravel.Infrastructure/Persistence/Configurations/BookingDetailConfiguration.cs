using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravel.Domain.Entities.Bookings;

namespace OnlineTravel.Infrastructure.Persistence.Configurations;

public class BookingDetailConfiguration : IEntityTypeConfiguration<BookingDetail>
{
    public void Configure(EntityTypeBuilder<BookingDetail> builder)
    {
        builder.ToTable("BookingDetails", "bookings");

        builder.HasIndex(e => new { e.BookingId, e.CategoryId });
        builder.HasIndex(e => e.ItemId);

        builder.OwnsOne(e => e.StayRange, dr =>
        {
            dr.Property(p => p.Start).HasColumnName("CheckInDate");
            dr.Property(p => p.End).HasColumnName("CheckOutDate");
        });

        builder.HasOne(e => e.Booking)
            .WithMany(b => b.Details)
            .HasForeignKey(e => e.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Category)
            .WithMany()
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}




