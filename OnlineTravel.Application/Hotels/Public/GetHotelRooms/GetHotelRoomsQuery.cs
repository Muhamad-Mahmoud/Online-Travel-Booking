using MediatR;
using OnlineTravel.Application.Hotels.Common;
using OnlineTravel.Application.Hotels.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Hotels.Public.GetHotelRooms
{
    public class GetHotelRoomsQuery : IRequest<Result<List<RoomDto>>>
    {
        public Guid HotelId { get; set; }
        public DateOnly? CheckIn { get; set; }
        public DateOnly? CheckOut { get; set; }
    }

}
