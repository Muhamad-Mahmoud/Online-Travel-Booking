using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Flight.Airport.UpdateAirport
{
    public class UpdateAirportCommand:IRequest<UpdateAirportResponse>
    {
        public Guid Id { get; set; } // Required to know which airport to update
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;

        // Address details
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;

        public List<string> Facilities { get; set; } = new List<string>();
    }
}
