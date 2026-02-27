using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravel.Domain.Entities.Reviews;

namespace OnlineTravel.Infrastructure.Persistence.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
	public void Configure(EntityTypeBuilder<Review> builder)
	{
		builder.ToTable("Reviews", "reviews");
		builder.HasIndex(e => new { e.UserId, e.CategoryId, e.ItemId }); // Allow multiple reviews
		builder.HasIndex(e => new { e.CategoryId, e.ItemId });

		builder.OwnsOne(e => e.Rating, r =>
		{
			r.Property(p => p.Value).HasColumnName("Rating").HasColumnType("decimal(2,1)");
		});

		builder.HasOne(e => e.User)
			.WithMany()
			.HasForeignKey(e => e.UserId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(e => e.Category)
			.WithMany()
			.HasForeignKey(e => e.CategoryId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(e => e.Booking)
			.WithMany()
			.HasForeignKey(e => e.BookingId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}
