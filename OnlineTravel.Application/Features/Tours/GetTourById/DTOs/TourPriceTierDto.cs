using OnlineTravel.Domain.Entities._Shared.ValueObjects;

namespace OnlineTravel.Application.Features.Tours.GetTourById.DTOs;

public class TourPriceTierDto
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public Money Price { get; set; } = null!;
	public string? Description { get; set; }
}
