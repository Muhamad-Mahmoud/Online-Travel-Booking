using OnlineTravel.Domain.Entities.Flights;

namespace OnlineTravel.Application.Specifications;

public class FlightFareByFlightSpec : BaseSpecification<FlightFare>
{
    public FlightFareByFlightSpec(Guid flightId)
        : base(f => f.FlightId == flightId)
    {
    }
}
