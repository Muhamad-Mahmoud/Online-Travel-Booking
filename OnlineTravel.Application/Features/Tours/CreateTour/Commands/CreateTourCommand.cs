using MediatR;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;

namespace OnlineTravel.Application.Features.Tours.CreateTour.Commands
{
    public class CreateTourCommand : IRequest<OnlineTravel.Domain.ErrorHandling.Result<Guid>>
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
        public int DurationDays { get; set; }
        public int DurationNights { get; set; }
        public bool Recommended { get; set; }
        public string? BestTimeToVisit { get; set; }

        // Address
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public string? MainImageUrl { get; set; }
        
        // Pricing (simplified for creation)
        public decimal StandardPrice { get; set; }
        public string Currency { get; set; } = "USD";
    }
}

