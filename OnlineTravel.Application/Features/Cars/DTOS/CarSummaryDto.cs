using OnlineTravel.Application.Features.CarPricingTiers.DTOs;
using OnlineTravel.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Cars.DTOs
{
    public class CarSummaryDto
    {
        public Guid Id { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public CarCategory CarType { get; set; }
        public int SeatsCount { get; set; }
        public FuelType FuelType { get; set; }
        public TransmissionType Transmission { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public string CategoryTitle { get; set; } = string.Empty;
        public string? MainImage { get; set; } // أول صورة
        public decimal PricePerHour { get; set; } // أقل سعر مثلاً
    }
}
