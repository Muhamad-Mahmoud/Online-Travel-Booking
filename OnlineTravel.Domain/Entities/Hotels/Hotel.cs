using OnlineTravel.Domain.Entities._Base;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Core;
using OnlineTravel.Domain.Entities.Reviews.ValueObjects;

namespace OnlineTravel.Domain.Entities.Hotels;

public class Hotel : SoftDeletableEntity
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public Address Address { get; set; } = null!;

    public ImageUrl? MainImage { get; set; }

    public List<ImageUrl> Gallery { get; set; } = new();

    public List<string> Amenities { get; set; } = new();

    public StarRating? StarRating { get; set; }

    public ContactInfo? ContactInfo { get; set; }

    public Guid CategoryId { get; set; }

    // Navigation Properties

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}





