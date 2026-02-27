using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravel.Domain.Entities.Flights;

namespace OnlineTravel.Infrastructure.Persistence.Configurations;

public class FlightFareConfiguration : IEntityTypeConfiguration<FlightFare>
{
	public void Configure(EntityTypeBuilder<FlightFare> builder)
	{
		builder.ToTable("FlightFares", "flights");

		builder.OwnsOne(e => e.BasePrice, m =>
		{
			m.Property(p => p.Amount).HasColumnName("BasePrice");
			m.Property(p => p.Currency).HasColumnName("Currency");
		});

		builder.HasIndex(e => e.FlightId);
	}
}




