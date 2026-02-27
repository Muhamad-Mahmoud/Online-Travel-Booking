using OnlineTravel.Domain.Entities.Flights;

namespace OnlineTravel.Application.Specifications.Flights
{
	public class FlightSearchSpecification : BaseSpecification<Flight>
	{
		public FlightSearchSpecification(Guid originId, Guid destinationId, DateTime date)
	 : base(f => f.OriginAirportId == originId &&
				 f.DestinationAirportId == destinationId &&
				 f.Schedule.Start.Date == date.Date)
		{
			AddIncludes(f => f.Carrier);

			AddOrderBy(f => f.Schedule.Start);
		}
	}
}
