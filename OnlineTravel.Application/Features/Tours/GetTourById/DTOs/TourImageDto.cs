namespace OnlineTravel.Application.Features.Tours.GetTourById.DTOs;

public class TourImageDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? AltText { get; set; }
}
