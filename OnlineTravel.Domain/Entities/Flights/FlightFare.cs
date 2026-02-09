using OnlineTravel.Domain.Entities._Base;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;

namespace OnlineTravel.Domain.Entities.Flights;

public class FlightFare : BaseEntity
{
    public Guid FlightId { get; set; }

    public Money BasePrice { get; set; } = null!;

    public int SeatsAvailable { get; set; }

    public Guid? FareRulesId { get; set; }


    // Navigation Properties

    public virtual Flight Flight { get; set; } = null!;

    public virtual FareRule? FareRules { get; set; }
}




