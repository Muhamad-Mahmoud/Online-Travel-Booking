using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Cars.DTOS
{
    public class CarResponseDto
    {
        public Guid Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public CarCategory CarType { get; set; }
        public int SeatsCount { get; set; }
        public FuelType FuelType { get; set; }
        public TransmissionType Transmission { get; set; }
        public List<string> Features { get; set; }
        public List<DateRange> AvailableDates { get; set; }
        public string? CancellationPolicy { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryTitle { get; set; }
        public Guid BrandId { get; set; }
        public string BrandName { get; set; }
        public List<CarPricingTierDto> PricingTiers { get; set; }
        public List<ImageUrlDto> Images { get; set; }
        public Point Location { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}
