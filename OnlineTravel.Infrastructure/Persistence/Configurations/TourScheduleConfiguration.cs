using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravel.Domain.Entities.Tours;

namespace OnlineTravel.Infrastructure.Persistence.Configurations;

public class TourScheduleConfiguration : IEntityTypeConfiguration<TourSchedule>
{
    public void Configure(EntityTypeBuilder<TourSchedule> builder)
    {
        builder.ToTable("TourSchedules", "tours");

        builder.OwnsOne(e => e.DateRange, dr =>
        {
            dr.Property(p => p.Start).HasColumnName("StartDate");
            dr.Property(p => p.End).HasColumnName("EndDate");
        });



        builder.HasOne(e => e.Tour)
            .WithMany()
            .HasForeignKey(e => e.TourId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}




