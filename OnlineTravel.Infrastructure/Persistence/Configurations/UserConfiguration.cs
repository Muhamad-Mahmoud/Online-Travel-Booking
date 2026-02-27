using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravel.Domain.Entities.Users;

namespace OnlineTravel.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<AppUser>
{
	public void Configure(EntityTypeBuilder<AppUser> builder)
	{
		builder.ToTable("Users", "identity");
		builder.HasIndex(e => e.Email).IsUnique();
		builder.HasIndex(e => e.PhoneNumber);

		builder.OwnsOne(e => e.Address, a =>
		{
			a.Property(p => p.FullAddress).HasColumnName("Address");
			a.Property(p => p.Coordinates).HasColumnName("Location");
		}).Navigation(e => e.Address).IsRequired();
	}


}
