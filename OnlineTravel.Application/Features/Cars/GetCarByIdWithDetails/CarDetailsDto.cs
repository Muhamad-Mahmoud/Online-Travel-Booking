using OnlineTravel.Application.Features.CarPricingTiers.Common;
using OnlineTravel.Application.Features.Cars.Shared.DTOs;
using OnlineTravel.Domain.Enums;
using System;
using System.Collections.Generic;

namespace OnlineTravel.Application.Features.Cars.GetCarByIdWithDetails
{
    public class CarDetailsDto
    {
        public Guid Id { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string? BrandName { get; set; }
        public string? CategoryTitle { get; set; }
        public CarCategory CarType { get; set; }
        public int SeatsCount { get; set; }
        public FuelType FuelType { get; set; }
        public TransmissionType Transmission { get; set; }
        public List<string> Features { get; set; } = new();
        public List<DateTimeRangeDto> AvailableDates { get; set; } = new();
        public string? CancellationPolicy { get; set; }
        public LocationDto Location { get; set; } = null!;
        public List<ImageUrlDto> Images { get; set; } = new();
        public List<CarPricingTierDto> PricingTiers { get; set; } = new();
        public string? MainImage { get; set; }
        public decimal PricePerHour { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
