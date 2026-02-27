using OnlineTravel.Domain.Entities._Base;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;

namespace OnlineTravel.Domain.Entities.Tours;

public class TourActivity : BaseEntity
{
	public string Title { get; set; } = string.Empty;

	public string Description { get; set; } = string.Empty;

	public ImageUrl Image { get; set; } = null!;

	public Guid TourId { get; set; }

	// Navigation Properties
	public virtual Tour Tour { get; set; } = null!;
}
