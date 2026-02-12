using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Hotel
{
    public record CreateHotelCommand : IRequest<CreateHotelResponse>
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string? State { get; set; }
        public string Country { get; set; } = string.Empty;
        public string? PostalCode { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Website { get; set; }



        public string MainImageUrl { get; set; } = string.Empty;


        public string? MainImageAlt { get; set; }

        public List<string> GalleryUrls { get; set; } = new();

        public decimal? StarRating { get; set; }

        public Guid CategoryId { get; set; }


    }
}