using OnlineTravel.Domain.Entities._Base;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Enums;

namespace OnlineTravel.Domain.Entities.Tours;

public class TourSchedule : BaseEntity
{
    public Guid TourId { get; set; }

    public DateRange DateRange { get; set; } = null!;


    public int TotalCapacity { get; set; }

    public int AvailableSlots { get; set; }

    public Guid PriceTierId { get; set; }

    public TourScheduleStatus Status { get; set; } = TourScheduleStatus.Active;

    public byte[]? RowVersion { get; set; }

    // Navigation Properties

    public virtual Tour Tour { get; set; } = null!;

    public virtual TourPriceTier PriceTier { get; set; } = null!;
}