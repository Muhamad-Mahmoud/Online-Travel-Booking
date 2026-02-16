using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravel.Domain.Entities.Payments;

namespace OnlineTravel.Infrastructure.Persistence.Configurations;

public class ProcessedWebhookEventConfiguration : IEntityTypeConfiguration<ProcessedWebhookEvent>
{
    public void Configure(EntityTypeBuilder<ProcessedWebhookEvent> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.EventId)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Provider)
            .IsRequired()
            .HasMaxLength(50);

        // Unique index on EventId to prevent duplicate processing at the database level
        builder.HasIndex(e => e.EventId)
            .IsUnique();
    }
}
