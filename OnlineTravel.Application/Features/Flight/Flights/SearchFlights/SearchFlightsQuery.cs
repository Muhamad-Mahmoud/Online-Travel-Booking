using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Flight.Flights.SearchFlights
{
    public class SearchFlightsQuery : IRequest<List<SearchFlightsDto>>
    {
        public Guid OriginAirportId { get; set; }
        public Guid DestinationAirportId { get; set; }
        public DateTime DepartureDate { get; set; }
        public int Passengers { get; set; } = 1; 
    }
}
