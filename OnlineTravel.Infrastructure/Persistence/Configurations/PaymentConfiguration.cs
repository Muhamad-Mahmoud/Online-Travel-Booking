using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravel.Domain.Entities.Payments;

namespace OnlineTravel.Infrastructure.Persistence.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
	public void Configure(EntityTypeBuilder<Payment> builder)
	{
		builder.ToTable("Payments", "billing");
		builder.HasIndex(e => e.TransactionId).IsUnique();
		builder.HasIndex(e => e.BookingId);

		builder.OwnsOne(e => e.Amount, m =>
		{
			m.Property(p => p.Amount).HasColumnName("Amount");
			m.Property(p => p.Currency).HasColumnName("Currency");
		});

		builder.OwnsOne(e => e.RefundAmount, m =>
		{
			m.Property(p => p.Amount).HasColumnName("RefundAmount");
			m.Property(p => p.Currency).HasColumnName("RefundCurrency");
		});

		builder.HasOne(e => e.Booking)
			.WithMany()
			.HasForeignKey(e => e.BookingId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}




