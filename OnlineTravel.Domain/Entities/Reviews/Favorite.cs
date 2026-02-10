using OnlineTravel.Domain.Entities._Base;
using OnlineTravel.Domain.Entities.Core;
using OnlineTravel.Domain.Entities.Users;

namespace OnlineTravel.Domain.Entities.Reviews;

public class Favorite : BaseEntity
{
    public Guid UserId { get; set; }

    public Guid CategoryId { get; set; }

    public Guid ItemId { get; set; }

    public DateTime AddedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties

    public virtual AppUser User { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;
}
