using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Hotels;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("Rooms", "hotels");

        builder.HasIndex(e => new { e.HotelId, e.RoomNumber });

        builder.Property(e => e.AvailableDates)
            .HasColumnName("AvailableDatesJson")
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<List<DateRange>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new List<DateRange>())
            .Metadata.SetValueComparer(new ValueComparer<List<DateRange>>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()));

        builder.Property(e => e.Extras)
            .HasColumnName("ExtrasJson")
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new List<string>())
            .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()));

        // ÅÖÇÝÉ ÇáÜ PricePerNight
        builder.OwnsOne(r => r.PricePerNight, pp =>
        {
            pp.Property(p => p.Amount).HasColumnName("PriceAmount");
            pp.Property(p => p.Currency).HasColumnName("PriceCurrency");
        });
    }
}
