using OnlineTravel.Application.Features.CarPricingTiers.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Cars.DTOs
{
    public class CarDetailsDto : CarSummaryDto
    {
        public List<string> Features { get; set; } = new();
        public List<DateTimeRangeDto> AvailableDates { get; set; } = new();
        public string? CancellationPolicy { get; set; }
        public LocationDto Location { get; set; } = null!;
        public List<ImageUrlDto> Images { get; set; } = new();
        public List<CarPricingTierDto> PricingTiers { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
