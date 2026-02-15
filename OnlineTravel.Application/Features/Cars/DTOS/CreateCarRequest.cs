using NetTopologySuite.Geometries;
using OnlineTravel.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Cars.DTOs
{
    public class CreateCarRequest
    {
        public Guid BrandId { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public CarCategory CarType { get; set; }
        public int SeatsCount { get; set; }
        public FuelType FuelType { get; set; }
        public TransmissionType Transmission { get; set; }
        public List<string> Features { get; set; } = new();
        public List<DateTimeRangeDto> AvailableDates { get; set; } = new();
        public string? CancellationPolicy { get; set; }
        public Guid CategoryId { get; set; }
        public LocationDto Location { get; set; } = null!;
        public List<ImageUrlDto> Images { get; set; } = new();
    }
}
