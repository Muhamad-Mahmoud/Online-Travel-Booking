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
        builder.ToTable("Hotels", "hotels");

        builder.OwnsOne(e => e.Address, a =>
        {
            a.Property(p => p.FullAddress).HasColumnName("Address");
            a.Property(p => p.Coordinates).HasColumnName("Location");
            a.Property(p => p.City).HasColumnName("City");
            a.Property(p => p.Country).HasColumnName("Country");
            a.Property(p => p.Street).HasColumnName("Street");
            a.Property(p => p.State).HasColumnName("State");
            a.Property(p => p.PostalCode).HasColumnName("PostalCode");
        });

        builder.OwnsOne(e => e.StarRating, r =>
        {
            r.Property(p => p.Value).HasColumnName("StarRating").HasColumnType("decimal(2,1)");
        });

        builder.OwnsOne(e => e.ContactInfo, ci =>
        {
            ci.OwnsOne(c => c.Email, e => e.Property(p => p.Value).HasColumnName("ContactEmail"));
            ci.OwnsOne(c => c.Phone, p => p.Property(p => p.Value).HasColumnName("ContactPhone"));
            ci.OwnsOne(c => c.Website, w => w.Property(p => p.Value).HasColumnName("ContactWebsite"));
        }).Navigation(e => e.ContactInfo).IsRequired();

        builder.OwnsOne(e => e.MainImage, i =>
        {
            i.Property(p => p.Url).HasColumnName("MainImageUrl");
            i.Property(p => p.AltText).HasColumnName("MainImageAlt");
        });

        builder.Property(e => e.Gallery)
            .HasColumnName("GalleryJson")
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<List<ImageUrl>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new List<ImageUrl>())
            .Metadata.SetValueComparer(new ValueComparer<List<ImageUrl>>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()));

        builder.HasOne(e => e.Category)
            .WithMany()
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Rooms)
            .WithOne(r => r.Hotel)
            .HasForeignKey(r => r.HotelId)
            .OnDelete(DeleteBehavior.Cascade);
    }


}




