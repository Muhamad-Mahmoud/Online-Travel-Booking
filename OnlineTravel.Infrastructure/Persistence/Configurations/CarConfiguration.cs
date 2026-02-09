using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Cars;

namespace OnlineTravel.Infrastructure.Persistence.Configurations;

public class CarConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.ToTable("Cars", "cars");
        builder.HasIndex(e => new { e.CarType, e.CategoryId });
        builder.Property(e => e.RowVersion).IsRowVersion();

        builder.HasOne(e => e.Category)
            .WithMany()
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.PricingTiers)
            .WithOne(p => p.Car)
            .HasForeignKey(p => p.CarId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Extras)
            .WithOne(e => e.Car)
            .HasForeignKey(e => e.CarId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(e => e.Location);

        // Configure Images collection
        builder.Property(e => e.Images)
            .HasColumnName("ImagesJson")
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<List<ImageUrl>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new List<ImageUrl>())
            .Metadata.SetValueComparer(new ValueComparer<List<ImageUrl>>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()));

        // Configure AvailableDates as JSON
        builder.Property(e => e.AvailableDates)
            .HasColumnName("AvailableDatesJson")
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<List<DateRange>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new List<DateRange>())
            .Metadata.SetValueComparer(new ValueComparer<List<DateRange>>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()));
    }

}