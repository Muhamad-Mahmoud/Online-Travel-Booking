using OnlineTravel.Domain.Entities._Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Domain.Entities.Hotels
{
    public class HotelGallery : BaseEntity
    {
        public Guid HotelId { get; set; }
        public string Url { get; set; }
        public string Alt { get; set; }

        public Hotel Hotel { get; set; }
    }
}
