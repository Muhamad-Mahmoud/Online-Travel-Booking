using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Flight.Airport.GetAllAirports
{
    public class GetAllAirportsQuery: IRequest<List<GetAllAirportsDto>>
    {
    }
}
