using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using OnlineTravel.Application.Features.Flight.Airport.CreateAirport;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Flight.CreateAirport
{
    public class CreateAirportCommand : IRequest<Result<CreateAirportResponse>>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public List<string> Facilities { get; set; }
    }
}

