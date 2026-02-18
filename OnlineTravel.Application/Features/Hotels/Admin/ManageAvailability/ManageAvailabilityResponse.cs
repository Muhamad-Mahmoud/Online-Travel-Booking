using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Hotels.Admin.ManageAvailability
{
    public class ManageAvailabilityResponse
    {
        public Guid RoomId { get; set; }
        public string Message { get; set; }
    }

}
