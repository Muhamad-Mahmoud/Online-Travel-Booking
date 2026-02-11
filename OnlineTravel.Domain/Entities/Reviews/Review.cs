using OnlineTravel.Domain.Entities._Base;
using OnlineTravel.Domain.Entities.Core;
using OnlineTravel.Domain.Entities.Reviews.ValueObjects;
using OnlineTravel.Domain.Entities.Users;

namespace OnlineTravel.Domain.Entities.Reviews;

public class Review : SoftDeletableEntity
{
    public Guid UserId { get; set; }

    public Guid CategoryId { get; set; }

    public Guid ItemId { get; set; }

    public StarRating Rating { get; set; } = null!;

    public string? Comment { get; set; }

    public Guid? BookingId { get; set; }

    // Navigation Properties

    public virtual User User { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual Bookings.BookingEntity? Booking { get; set; }
}




