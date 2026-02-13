using MediatR;
using OnlineTravel.Application.Features.Cars.DTOS;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Cars.Commands
{
    public class CreateCarCommand : IRequest<Guid>
    {
        public Guid BrandId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public CarCategory CarType { get; set; }
        public int SeatsCount { get; set; }
        public FuelType FuelType { get; set; }
        public TransmissionType Transmission { get; set; }
        public List<string> Features { get; set; } = new();
        public List<DateRange> AvailableDates { get; set; } = new();
        public string? CancellationPolicy { get; set; }
        public Guid CategoryId { get; set; }
        public Point Location { get; set; }
        public List<CreateCarPricingTierDto> PricingTiers { get; set; } = new();
        public List<CreateImageUrlDto> Images { get; set; } = new();
    }
}
