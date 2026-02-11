using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravel.Domain.Entities.Core;

namespace OnlineTravel.Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories", "infra");
        //builder.HasIndex(e => e.Key).IsUnique();

        builder.OwnsOne(e => e.Image, i =>
        {
            i.Property(p => p.Url).HasColumnName("ImageUrl");
            i.Property(p => p.AltText).HasColumnName("ImageAlt");
        });


    }
}




