using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Hotels.Dtos
{
    public class HotelDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string MainImage { get; set; }
        public List<GalleryImageDto> Gallery { get; set; }
        public decimal Rating { get; set; }
        public int TotalReviews { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public TimeOnly CheckInTime { get; set; }
        public TimeOnly CheckOutTime { get; set; }
        public string CancellationPolicy { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string Website { get; set; }
        public List<ReviewDto> Reviews { get; set; }
    }

}
