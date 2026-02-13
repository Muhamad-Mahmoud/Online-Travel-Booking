using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravel.Domain.Entities.Flights;

namespace OnlineTravel.Infrastructure.Persistence.Configurations;

public class FlightConfiguration : IEntityTypeConfiguration<Flight>
{
    public void Configure(EntityTypeBuilder<Flight> builder)
    {
        builder.ToTable("Flights", "flights");
        builder.Property(e => e.RowVersion).IsRowVersion();

        builder.Property(e => e.RowVersion).IsRowVersion();


        builder.OwnsOne(e => e.FlightNumber, fn =>
        {
            fn.Property(p => p.Value).HasColumnName("FlightNumber");
        });

        builder.OwnsOne(e => e.Schedule, s =>
        {
            s.Property(p => p.Start).HasColumnName("DepartureAt");
            s.Property(p => p.End).HasColumnName("ArrivalAt");
        });


        // Configure relationships to airports
        builder.HasOne(e => e.OriginAirport)
            .WithMany(a => a.DepartingFlights)
            .HasForeignKey(e => e.OriginAirportId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.DestinationAirport)
            .WithMany(a => a.ArrivingFlights)
            .HasForeignKey(e => e.DestinationAirportId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Category)
            .WithMany()
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Fares)
            .WithOne(f => f.Flight)
            .HasForeignKey(f => f.FlightId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Seats)
            .WithOne(s => s.Flight)
            .HasForeignKey(s => s.FlightId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(e => e.BaggageRules)
            .HasColumnName("BaggageRulesJson")
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new List<string>())
            .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()));
    }
}




