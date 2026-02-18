using OnlineTravel.Domain.Entities._Shared.ValueObjects;

namespace OnlineTravel.Application.Features.Tours.GetTourById.DTOs;

public class TourDetailsResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public bool Recommended { get; set; }
    public Address? Location { get; set; }
    public int DurationDays { get; set; }
    public int DurationNights { get; set; }
    public double Rating { get; set; }
    public int ReviewCount { get; set; }
    public string MainImageUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<TourActivityDto> Activities { get; set; } = new();
    public string BestTimeToVisit { get; set; } = string.Empty;
    public List<TourImageDto> Images { get; set; } = new();
    public List<TourPriceTierDto> PriceTiers { get; set; } = new();
    public PriceDto? Price { get; set; }
}
