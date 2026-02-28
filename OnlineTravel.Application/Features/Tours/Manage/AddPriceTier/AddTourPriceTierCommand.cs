using MediatR;

namespace OnlineTravel.Application.Features.Tours.Manage.AddPriceTier;

public class AddTourPriceTierCommand : IRequest<Domain.ErrorHandling.Result<Guid>>
{
	public Guid TourId { get; set; }
	public string Name { get; set; } = string.Empty;
	public decimal Amount { get; set; }
	public string Currency { get; set; } = "USD";
	public string? Description { get; set; }
}
