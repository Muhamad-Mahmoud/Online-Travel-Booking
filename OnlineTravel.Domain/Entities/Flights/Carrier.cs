using OnlineTravel.Domain.Entities._Base;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Flights.ValueObjects;

namespace OnlineTravel.Domain.Entities.Flights;

public class Carrier : BaseEntity
{
	public string Name { get; set; } = string.Empty;

	public IataCode Code { get; set; } = null!;


	public string? Logo { get; set; }

	public ContactInfo? ContactInfo { get; set; }

	public bool IsActive { get; set; } = true;


	// Navigation Properties

	public virtual ICollection<Flight> Flights { get; set; } = new List<Flight>();
}




