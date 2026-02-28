namespace OnlineTravel.Application.Features.Tours.GetTourById.DTOs;

public class PriceResponse
{
	public decimal Amount { get; set; }
	public string Currency { get; set; } = string.Empty;
}
