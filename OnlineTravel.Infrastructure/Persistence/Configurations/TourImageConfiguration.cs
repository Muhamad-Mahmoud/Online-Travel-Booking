using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravel.Domain.Entities.Tours;

namespace OnlineTravel.Infrastructure.Persistence.Configurations;

public class TourImageConfiguration : IEntityTypeConfiguration<TourImage>
{
	public void Configure(EntityTypeBuilder<TourImage> builder)
	{
		builder.ToTable("TourImages", "tours");

		builder.Property(x => x.Url).IsRequired();
		builder.Property(x => x.AltText).HasMaxLength(200);

		builder.HasOne(x => x.Tour)
			.WithMany(x => x.Images)
			.HasForeignKey(x => x.TourId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
