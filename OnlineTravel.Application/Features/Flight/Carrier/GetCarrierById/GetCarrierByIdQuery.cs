using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Flight.Carrier.GetCarrierById
{
    public class GetCarrierByIdQuery : IRequest<GetCarrierByIdDto>
    {
        public Guid Id { get; set; }
        public GetCarrierByIdQuery(Guid id) => Id = id;
    }
}
