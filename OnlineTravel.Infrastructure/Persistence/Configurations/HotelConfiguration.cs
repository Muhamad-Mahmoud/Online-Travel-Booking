using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Hotels;

namespace OnlineTravel.Infrastructure.Persistence.Configurations;

public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        // Primary Key
        builder.HasKey(h => h.Id);

        // Basic Properties
        builder.Property(h => h.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(h => h.Slug)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(h => h.Slug).IsUnique();

        builder.Property(h => h.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(h => h.CancellationPolicy)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(h => h.MainImageUrl)
            .HasMaxLength(500);

        // Address owned type
        builder.OwnsOne(h => h.Address, address =>
        {
            address.Property(a => a.Street).HasMaxLength(200);
            address.Property(a => a.City).IsRequired().HasMaxLength(100);
            address.Property(a => a.State).HasMaxLength(100);
            address.Property(a => a.Country).IsRequired().HasMaxLength(100);
            address.Property(a => a.PostalCode).HasMaxLength(20);
            address.Property(a => a.Coordinates).HasColumnType("geography");
        });

        // ContactInfo owned type
        builder.OwnsOne(h => h.ContactInfo, contact =>
        {
            // EmailAddress conversion
            contact.Property(c => c.Email)
                   .HasMaxLength(200)
                   .HasConversion(
                       v => v.Value,            // EmailAddress -> string
                       v => new EmailAddress(v) // string -> EmailAddress
                   );

            contact.Ignore(c => c.Website); 


            // PhoneNumber inside ContactInfo
            contact.OwnsOne(c => c.Phone, phone =>
            {
                phone.Property(p => p.Value) // EF maps Value only
                     .IsRequired()
                     .HasMaxLength(20)
                     .HasColumnName("PhoneNumber");
            });
        });


        // CheckInTime owned type
        builder.OwnsOne(h => h.CheckInTime, time =>
        {
            time.Property(t => t.Start)
                .HasConversion(
                    v => v.ToTimeSpan(),
                    v => TimeOnly.FromTimeSpan(v))
                .IsRequired()
                .HasColumnName("CheckInStart");

            time.Property(t => t.End)
                .HasConversion(
                    v => v.ToTimeSpan(),
                    v => TimeOnly.FromTimeSpan(v))
                .IsRequired()
                .HasColumnName("CheckInEnd");
        });

        // CheckOutTime owned type
        builder.OwnsOne(h => h.CheckOutTime, time =>
        {
            time.Property(t => t.Start)
                .HasConversion(
                    v => v.ToTimeSpan(),
                    v => TimeOnly.FromTimeSpan(v))
                .IsRequired()
                .HasColumnName("CheckOutStart");

            time.Property(t => t.End)
                .HasConversion(
                    v => v.ToTimeSpan(),
                    v => TimeOnly.FromTimeSpan(v))
                .IsRequired()
                .HasColumnName("CheckOutEnd");
        });

        // Rating owned type
        // Rating owned type
        builder.OwnsOne(h => h.Rating, rating =>
        {
            rating.Property(r => r.Value)
                  .HasPrecision(5, 2); // 5 ÃÑÞÇã ÅÌãÇáí¡ 2 ÑÞã ÚÔÑí
        });


        // Relationships
        builder.HasMany(h => h.Rooms)
               .WithOne(r => r.Hotel)
               .HasForeignKey(r => r.HotelId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(h => h.Reviews)
               .WithOne(r => r.Hotel)
               .HasForeignKey(r => r.HotelId)
               .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(h => h.CreatedAt);
        builder.HasIndex(h => h.Name);
    }
}
