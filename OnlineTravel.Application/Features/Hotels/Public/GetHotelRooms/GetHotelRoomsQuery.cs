using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Application.Features.Hotels.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Hotels.Public.GetHotelRooms
{
    public class GetHotelRoomsQuery : IRequest<Result<List<RoomDto>>>
    {
        public Guid HotelId { get; set; }
        public DateOnly? CheckIn { get; set; }
        public DateOnly? CheckOut { get; set; }
    }

}
