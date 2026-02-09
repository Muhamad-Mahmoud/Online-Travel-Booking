using OnlineTravel.Domain.Entities._Base;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;

namespace OnlineTravel.Domain.Entities.Cars;

public class CarExtra : BaseEntity
{
    public Guid CarId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public Money? PricePerDay { get; set; }

    public Money? PricePerRental { get; set; }

    public bool Available { get; set; } = true;

    // Navigation Properties

    public virtual Car Car { get; set; } = null!;
}




