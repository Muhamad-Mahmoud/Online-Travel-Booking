using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Flight.Flights.CreateFlight
{
    public class CreateFlightCommand: IRequest<Guid>
    {
        public string FlightNumber { get; set; } = string.Empty;
        public Guid CarrierId { get; set; }
        public Guid OriginAirportId { get; set; }
        public Guid DestinationAirportId { get; set; }

        // Schedule details
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        public List<string> BaggageRules { get; set; } = new();
        public bool Refundable { get; set; }
        public Guid CategoryId { get; set; }
    }
}
