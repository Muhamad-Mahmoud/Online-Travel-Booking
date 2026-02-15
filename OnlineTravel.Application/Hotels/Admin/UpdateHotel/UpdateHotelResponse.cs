using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Hotels.Admin.UpdateHotel
{
    public class UpdateHotelResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}
