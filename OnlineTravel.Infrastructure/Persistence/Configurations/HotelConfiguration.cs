using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravel.Domain.Entities.Hotels;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using NetTopologySuite.Geometries;

namespace OnlineTravel.Infrastructure.Persistence.Configurations;

public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.HasKey(h => h.Id);

        builder.Property(h => h.Name).IsRequired().HasMaxLength(200);
        builder.Property(h => h.Slug).IsRequired().HasMaxLength(200);
        builder.HasIndex(h => h.Slug).IsUnique();
        builder.Property(h => h.Description).IsRequired().HasMaxLength(2000);
        builder.Property(h => h.CancellationPolicy).IsRequired().HasMaxLength(1000);
        builder.Property(h => h.MainImageUrl).HasMaxLength(500);

        builder.OwnsOne(h => h.Address, a =>
        {
            a.Property(p => p.Street).HasMaxLength(200);
            a.Property(p => p.City).IsRequired().HasMaxLength(100);
            a.Property(p => p.State).HasMaxLength(100);
            a.Property(p => p.Country).IsRequired().HasMaxLength(100);
            a.Property(p => p.PostalCode).HasMaxLength(20);
            a.Property(p => p.Coordinates).HasColumnType("geography");
            a.Property(p => p.FullAddress).HasMaxLength(500);
        });

        builder.OwnsOne(h => h.ContactInfo, c =>
        {
            c.Property(p => p.Email)
             .HasColumnName("ContactEmail")
             .HasMaxLength(200)
             .HasConversion(v => v != null ? v.Value : null, v => v != null ? new EmailAddress(v) : null);

            c.Property(p => p.Phone)
             .HasColumnName("PhoneNumber")
             .HasMaxLength(20)
             .HasConversion(v => v != null ? v.Value : null, v => v != null ? new PhoneNumber(v) : null);

            c.Property(p => p.Website)
             .HasColumnName("Website")
             .HasMaxLength(500)
             .HasConversion(v => v != null ? v.Value : null, v => v != null ? new Url(v) : null);
        });

        builder.OwnsOne(h => h.CheckInTime, t =>
        {
            t.Property(p => p.Start).HasConversion(v => v.ToTimeSpan(), v => TimeOnly.FromTimeSpan(v)).HasColumnName("CheckInStart");
            t.Property(p => p.End).HasConversion(v => v.ToTimeSpan(), v => TimeOnly.FromTimeSpan(v)).HasColumnName("CheckInEnd");
        });

        builder.OwnsOne(h => h.CheckOutTime, t =>
        {
            t.Property(p => p.Start).HasConversion(v => v.ToTimeSpan(), v => TimeOnly.FromTimeSpan(v)).HasColumnName("CheckOutStart");
            t.Property(p => p.End).HasConversion(v => v.ToTimeSpan(), v => TimeOnly.FromTimeSpan(v)).HasColumnName("CheckOutEnd");
        });

        builder.OwnsOne(h => h.Rating, r =>
        {
            r.Property(p => p.Value).HasPrecision(5, 2);
        });

        builder.HasMany(h => h.Rooms).WithOne(r => r.Hotel).HasForeignKey(r => r.HotelId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(h => h.Reviews).WithOne(r => r.Hotel).HasForeignKey(r => r.HotelId).OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(h => h.CreatedAt);
        builder.HasIndex(h => h.Name);
    }
}
