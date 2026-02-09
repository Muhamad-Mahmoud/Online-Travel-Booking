using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravel.Domain.Entities.Flights;

namespace OnlineTravel.Infrastructure.Persistence.Configurations;

public class AirportConfiguration : IEntityTypeConfiguration<Airport>
{
    public void Configure(EntityTypeBuilder<Airport> builder)
    {
        builder.ToTable("Airports", "flights");

        builder.OwnsOne(e => e.Code, c =>
        {
            c.Property(p => p.Value).HasColumnName("Code");
        });

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
    }
}