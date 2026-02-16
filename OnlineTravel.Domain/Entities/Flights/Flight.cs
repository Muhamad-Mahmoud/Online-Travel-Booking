using OnlineTravel.Domain.Entities._Base;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Core;
using OnlineTravel.Domain.Entities.Flights.ValueObjects;
using OnlineTravel.Domain.Enums;

namespace OnlineTravel.Domain.Entities.Flights;

public class Flight : SoftDeletableEntity
{
    public FlightNumber FlightNumber { get; set; } = null!;

    public Guid CarrierId { get; set; }

    public Guid OriginAirportId { get; set; }

    public Guid DestinationAirportId { get; set; }
    public virtual FlightMetadata? Metadata { get; set; }
    public DateTimeRange Schedule { get; set; } = null!;

    public List<string> BaggageRules { get; set; } = new();

    public bool Refundable { get; set; } = false;

    public FlightStatus Status { get; set; } = FlightStatus.Scheduled;

    public Guid CategoryId { get; set; }


    // Navigation Properties

    public virtual Carrier Carrier { get; set; } = null!;

    public virtual Airport OriginAirport { get; set; } = null!;

    public virtual Airport DestinationAirport { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<FlightFare> Fares { get; set; } = new List<FlightFare>();

    public virtual ICollection<FlightSeat> Seats { get; set; } = new List<FlightSeat>();
}




