using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Flight.Airport.UpdateAirport
{
    public class UpdateAirportResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }
    }
}
