using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Flight.Airport.GetAirportById
{
    public class GetAirportByIdQuery: IRequest<GetAirportByIdDto>
    {
        public Guid Id { get; set; }

        public GetAirportByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
