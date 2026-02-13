using OnlineTravel.Domain.Entities.Flights;
using OnlineTravel.Application.Specifications;

namespace OnlineTravel.Application.Features.Bookings.Specifications.Pricing;

public class FlightFareByFlightSpec : BaseSpecification<FlightFare>
{
    public FlightFareByFlightSpec(Guid flightId)
        : base(f => f.FlightId == flightId)
    {
    }
}
