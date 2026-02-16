using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Flight.Airport.GetAirportById
{
    public class GetAirportByIdDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string FullAddress { get; set; } // We can combine address fields here
        public List<string> Facilities { get; set; }
    }
}
