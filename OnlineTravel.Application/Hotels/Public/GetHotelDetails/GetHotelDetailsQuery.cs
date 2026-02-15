using MediatR;
using OnlineTravel.Application.Hotels.Common;
using OnlineTravel.Application.Hotels.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Hotels.Public.GetHotelDetails
{
    public class GetHotelDetailsQuery : IRequest<Result<HotelDetailsDto>>
    {
        public Guid Id { get; set; }
    }

}
