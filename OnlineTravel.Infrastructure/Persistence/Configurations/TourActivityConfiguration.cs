using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravel.Domain.Entities.Tours;

namespace OnlineTravel.Infrastructure.Persistence.Configurations;

public class TourActivityConfiguration : IEntityTypeConfiguration<TourActivity>
{
	public void Configure(EntityTypeBuilder<TourActivity> builder)
	{
		builder.ToTable("TourActivities");

		builder.HasKey(t => t.Id);

		builder.Property(t => t.Title)
			.IsRequired()
			.HasMaxLength(200);

		builder.Property(t => t.Description)
			.HasMaxLength(2000);

		builder.OwnsOne(t => t.Image, image =>
		{
			image.Property(i => i.Url).HasColumnName("ImageUrl").IsRequired();
			image.Property(i => i.AltText).HasColumnName("ImageAlt").HasMaxLength(200);
		});

		builder.HasOne(t => t.Tour)
			.WithMany(t => t.Activities)
			.HasForeignKey(t => t.TourId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
