using OnlineTravel.Domain.Entities._Base;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Enums;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Domain.Entities.Flights;

public class FlightSeat : BaseEntity
{
    public Guid FlightId { get; set; }

    public string SeatLabel { get; set; } = string.Empty;

    public CabinClass CabinClass { get; set; }

    public List<string> SeatFeatures { get; set; } = new();

    public bool IsAvailable { get; set; } = true;

    public Money? ExtraCharge { get; set; }

    public byte[]? RowVersion { get; set; }
    public DateTime? LastReservedAt { get; set; }

    public void Reserve()
    {
        if (!IsAvailable)
        {
            throw new DomainException($"Seat {SeatLabel} is no longer available.");
        }

        IsAvailable = false;
        LastReservedAt = DateTime.UtcNow;
    }

    // Navigation Properties

    public virtual Flight Flight { get; set; } = null!;
}




