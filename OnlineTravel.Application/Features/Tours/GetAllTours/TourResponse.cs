namespace OnlineTravel.Application.Features.Tours.GetAllTours;

public class TourResponse
{
	public Guid Id { get; set; }
	public string Title { get; set; } = string.Empty;
	public string ImageUrl { get; set; } = string.Empty;
	public string Category { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public string City { get; set; } = string.Empty;
	public string Country { get; set; } = string.Empty;
	public double Rating { get; set; }
	public decimal Price { get; set; }
	public string Currency { get; set; } = "USD";
	public bool IsFavorite { get; set; }
}
