using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravel.Domain.Entities.Flights;

namespace OnlineTravel.Infrastructure.Persistence.Configurations;

public class CarrierConfiguration : IEntityTypeConfiguration<Carrier>
{
    public void Configure(EntityTypeBuilder<Carrier> builder)
    {
        builder.ToTable("Carriers", "flights");

        builder.OwnsOne(e => e.Code, c =>
        {
            c.Property(p => p.Value).HasColumnName("Code");
        });

        builder.OwnsOne(e => e.ContactInfo, ci =>
        {
            ci.OwnsOne(c => c.Email, e => e.Property(p => p.Value).HasColumnName("ContactEmail"));
            ci.OwnsOne(c => c.Phone, p => p.Property(p => p.Value).HasColumnName("ContactPhone"));
            ci.OwnsOne(c => c.Website, w => w.Property(p => p.Value).HasColumnName("WebsiteUrl"));
        }).Navigation(e => e.ContactInfo).IsRequired();
    }


}




