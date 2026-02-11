namespace OnlineTravel.Application.Features.Tours.GetTourById.DTOs;

public class TourDetailsResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;
    public double Rating { get; set; }
    public int ReviewCount { get; set; }
    public string MainImageUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<TourActivityDto> TopActivities { get; set; } = new();
    public string BestTimeToVisit { get; set; } = string.Empty;
    public List<string> Gallery { get; set; } = new();
    public PriceDto? Price { get; set; }
}
