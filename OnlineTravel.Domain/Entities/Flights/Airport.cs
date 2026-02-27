using OnlineTravel.Domain.Entities._Base;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Flights.ValueObjects;

namespace OnlineTravel.Domain.Entities.Flights;

public class Airport : BaseEntity
{
	public IataCode Code { get; set; } = null!;

	public string Name { get; set; } = string.Empty;


	public Address Address { get; set; } = null!;


	public List<string> Facilities { get; set; } = new();

	public bool IsActive { get; set; } = true;

	// Navigation Properties

	public virtual ICollection<Flight> DepartingFlights { get; set; } = new List<Flight>();

	public virtual ICollection<Flight> ArrivingFlights { get; set; } = new List<Flight>();
}




