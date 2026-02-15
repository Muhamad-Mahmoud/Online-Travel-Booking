using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Tours;

namespace OnlineTravel.Infrastructure.Persistence.Configurations;

public class TourConfiguration : IEntityTypeConfiguration<Tour>
{
    public void Configure(EntityTypeBuilder<Tour> builder)
    {
        builder.ToTable("Tours", "tours");


        builder.OwnsOne(e => e.MainImage, i =>
        {
            i.Property(p => p.Url).HasColumnName("MainImageUrl");
            i.Property(p => p.AltText).HasColumnName("MainImageAlt");
        });




        builder.OwnsOne(e => e.Address, a =>
        {
            a.Property(p => p.FullAddress).HasColumnName("Address");
            a.Property(p => p.Coordinates).HasColumnName("Location");
            a.Property(p => p.Street).HasColumnName("Street");
            a.Property(p => p.City).HasColumnName("City");
            a.Property(p => p.State).HasColumnName("State");
            a.Property(p => p.Country).HasColumnName("Country");
            a.Property(p => p.PostalCode).HasColumnName("PostalCode");
        });


        builder.HasOne(e => e.Category)
            .WithMany()
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.PriceTiers)
            .WithOne(p => p.Tour)
            .HasForeignKey(p => p.TourId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Reviews)
            .WithOne()
            .HasForeignKey(r => r.ItemId)
            .OnDelete(DeleteBehavior.Cascade);


        // JSON Property Conversions
        builder.Property(e => e.Highlights)
            .HasColumnName("HighlightsJson")
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new List<string>())
            .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()));

        builder.Property(e => e.Tags)
            .HasColumnName("TagsJson")
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new List<string>())
            .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()));
    }
}




