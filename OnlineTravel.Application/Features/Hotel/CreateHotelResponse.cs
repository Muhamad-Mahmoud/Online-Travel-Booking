using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Hotel
{
    public class CreateHotelResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string MainImageUrl { get; set; } = string.Empty;
        public decimal? StarRating { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
