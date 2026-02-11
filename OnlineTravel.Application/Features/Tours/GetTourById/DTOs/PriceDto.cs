namespace OnlineTravel.Application.Features.Tours.GetTourById.DTOs;

public class PriceDto
{
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
}
