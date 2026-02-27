using OnlineTravel.Application.Specifications;
using OnlineTravel.Domain.Entities.Flights;

namespace OnlineTravel.Application.Features.Bookings.Specifications.Pricing;

public class FlightFareByFlightSpec : BaseSpecification<FlightFare>
{
	public FlightFareByFlightSpec(Guid flightId)
		: base(f => f.FlightId == flightId)
	{
	}
}
