using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Application.Features.Hotels.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Hotels.Admin.AddRoom
{
    public class AddRoomCommand : IRequest<Result<AddRoomResponse>>
    {
        public Guid HotelId { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal BasePricePerNight { get; set; }
        public List<PhotoDto>? Photos { get; set; }
    }

}
