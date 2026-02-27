using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravel.Domain.Entities.Hotels;

namespace OnlineTravel.Infrastructure.Persistence.Configurations
{
	public class RoomAvailabilityConfiguration : IEntityTypeConfiguration<RoomAvailability>
	{
		public void Configure(EntityTypeBuilder<RoomAvailability> builder)
		{
			builder.HasKey(ra => ra.Id);

			builder.Property(ra => ra.IsAvailable)
				.IsRequired();

			builder.OwnsOne(ra => ra.DateRange, dr =>
			{
				dr.Property(d => d.Start).IsRequired();
				dr.Property(d => d.End).IsRequired();
			});
		}
	}

}
