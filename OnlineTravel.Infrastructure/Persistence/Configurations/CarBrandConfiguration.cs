using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravel.Domain.Entities.Cars;

namespace OnlineTravel.Infrastructure.Persistence.Configurations;

public class CarBrandConfiguration : IEntityTypeConfiguration<CarBrand>
{
    public void Configure(EntityTypeBuilder<CarBrand> builder)
    {
        builder.ToTable("CarBrands", "cars");
        builder.HasIndex(e => e.Name).IsUnique();
    }
}




