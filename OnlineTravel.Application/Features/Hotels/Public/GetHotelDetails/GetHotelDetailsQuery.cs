using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Application.Features.Hotels.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Hotels.Public.GetHotelDetails
{
    public class GetHotelDetailsQuery : IRequest<Result<HotelDetailsDto>>
    {
        public Guid Id { get; set; }
    }

}
