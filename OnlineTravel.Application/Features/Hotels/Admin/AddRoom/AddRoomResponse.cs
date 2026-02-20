using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Hotels.Admin.AddRoom
{
    public class AddRoomResponse
    {
        public Guid Id { get; set; }
        public Guid HotelId { get; set; }
        public string Name { get; set; }
    }

}
