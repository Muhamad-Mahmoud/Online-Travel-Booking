using OnlineTravel.Domain.Entities._Base;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;

namespace OnlineTravel.Domain.Entities.Cars;

public class CarPricingTier : BaseEntity
{
    public Guid CarId { get; set; }

    public int FromHours { get; set; }

    public int ToHours { get; set; }

    public Money PricePerHour { get; set; } = null!;

    // Navigation Properties
    public virtual Car Car { get; set; } = null!;
}




