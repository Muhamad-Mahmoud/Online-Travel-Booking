using OnlineTravel.Domain.Entities._Base;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Enums;

namespace OnlineTravel.Domain.Entities.Flights;

public class FlightSeat : BaseEntity
{
    public Guid FlightId { get; set; }

    public string SeatLabel { get; set; } = string.Empty;

    public CabinClass CabinClass { get; set; }

    public List<string> SeatFeatures { get; set; } = new();

    public bool IsAvailable { get; set; } = true;

    public Money? ExtraCharge { get; set; }

    // Navigation Properties

    public virtual Flight Flight { get; set; } = null!;
}




