namespace OnlineTravel.Domain.Entities._Base;

public abstract class BaseEntity
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}




