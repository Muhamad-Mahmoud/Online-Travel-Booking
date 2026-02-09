using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravel.Domain.Entities.Flights;

namespace OnlineTravel.Infrastructure.Persistence.Configurations;

public class FlightSeatConfiguration : IEntityTypeConfiguration<FlightSeat>
{
    public void Configure(EntityTypeBuilder<FlightSeat> builder)
    {
        builder.ToTable("FlightSeats", "flights");
        builder.HasIndex(e => new { e.FlightId, e.SeatLabel }).IsUnique();

        builder.OwnsOne(e => e.ExtraCharge, m =>
        {
            m.Property(p => p.Amount).HasColumnName("ExtraCharge");
            m.Property(p => p.Currency).HasColumnName("Currency");
        });

        builder.Property(e => e.SeatFeatures)
            .HasColumnName("SeatFeaturesJson")
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new List<string>())
            .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()));
    }

}




