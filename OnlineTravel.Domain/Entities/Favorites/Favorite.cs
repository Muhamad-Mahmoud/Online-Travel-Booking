using OnlineTravel.Domain.Entities._Base;
using OnlineTravel.Domain.Entities.Users;
using OnlineTravel.Domain.Enums;

namespace OnlineTravel.Domain.Entities.Favorites;

public class Favorite : BaseEntity
{
	public Guid UserId { get; set; }
	public Guid ItemId { get; set; }
	public CategoryType ItemType { get; set; }

	public DateTime AddedAt { get; set; } = DateTime.UtcNow;

	// Navigation Properties
	public virtual AppUser User { get; set; } = null!;
}
