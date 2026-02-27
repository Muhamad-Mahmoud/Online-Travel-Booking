using OnlineTravel.Domain.Entities._Base;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;

namespace OnlineTravel.Domain.Entities.Tours;

public class TourPriceTier : BaseEntity
{
	public Guid TourId { get; set; }

	public string Name { get; set; } = string.Empty;

	public Money Price { get; set; } = null!;

	public string? Description { get; set; }

	public bool IsActive { get; set; } = true;

	// Navigation Properties

	public virtual Tour Tour { get; set; } = null!;

	public virtual ICollection<TourSchedule> Schedules { get; set; } = new List<TourSchedule>();
}
