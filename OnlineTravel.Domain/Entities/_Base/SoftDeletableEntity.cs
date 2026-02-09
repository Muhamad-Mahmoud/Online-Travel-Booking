namespace OnlineTravel.Domain.Entities._Base;

public abstract class SoftDeletableEntity : BaseEntity
{
    public DateTime? DeletedAt { get; set; }
}





