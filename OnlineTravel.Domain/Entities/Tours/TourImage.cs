using OnlineTravel.Domain.Entities._Base;

namespace OnlineTravel.Domain.Entities.Tours;

public class TourImage : BaseEntity
{
	public string Url { get; set; } = string.Empty;
	public string? AltText { get; set; }

	public Guid TourId { get; set; }
	public virtual Tour Tour { get; set; } = null!;
}
