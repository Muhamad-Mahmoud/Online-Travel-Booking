using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravel.Domain.Entities.Flights;

namespace OnlineTravel.Infrastructure.Persistence.Configurations;

public class FareRuleConfiguration : IEntityTypeConfiguration<FareRule>
{
    public void Configure(EntityTypeBuilder<FareRule> builder)
    {
        builder.ToTable("FareRules", "flights");
    }
}




