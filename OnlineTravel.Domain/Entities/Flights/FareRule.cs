using OnlineTravel.Domain.Entities._Base;

namespace OnlineTravel.Domain.Entities.Flights;

public class FareRule : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public List<string> CancellationRules { get; set; } = new();

    public bool IsActive { get; set; } = true;

    // Navigation Properties

    public virtual ICollection<FlightFare> FlightFares { get; set; } = new List<FlightFare>();
}




