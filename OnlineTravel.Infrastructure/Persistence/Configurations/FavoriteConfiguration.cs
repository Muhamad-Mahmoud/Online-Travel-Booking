using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravel.Domain.Entities.Favorites;

namespace OnlineTravel.Infrastructure.Persistence.Configurations;

public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
{
	public void Configure(EntityTypeBuilder<Favorite> builder)
	{
		builder.ToTable("Favorites"); // Use default schema or specify if needed

		builder.HasKey(f => f.Id);

		builder.Property(f => f.ItemType)
			.IsRequired()
			.HasMaxLength(50)
			.HasConversion<string>(); // Store as string for readability

		builder.Property(f => f.UserId)
			.IsRequired();

		builder.Property(f => f.ItemId)
			.IsRequired();

		// Ensure a user can only favorite an item once
		builder.HasIndex(f => new { f.UserId, f.ItemId, f.ItemType })
			.IsUnique();

		builder.HasOne(f => f.User)
			.WithMany() // Assuming AppUser doesn't have a Favorites collection yet
			.HasForeignKey(f => f.UserId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
