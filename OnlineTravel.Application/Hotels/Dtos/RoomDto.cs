using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Hotels.Dtos
{
    public class RoomDto
    {
        public Guid Id { get; set; }
        public Guid HotelId { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal BasePricePerNight { get; set; }
        public string Currency { get; set; } = "USD";
        public int Capacity { get; set; }
        public int BedCount { get; set; }
        public List<string> Photos { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public decimal? TotalPrice { get; set; }
    }



}
