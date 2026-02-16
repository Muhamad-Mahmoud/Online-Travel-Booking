using OnlineTravel.Domain.Entities._Base;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Core;

namespace OnlineTravel.Domain.Entities.Tours;

public class Tour : SoftDeletableEntity
{
    public string Title { get; set; } = string.Empty;

    public ImageUrl? MainImage { get; set; }

    public string? Description { get; set; }

    public Address Address { get; set; } = null!;

    public List<string> Highlights { get; set; } = new();

    public List<string> Tags { get; set; } = new();

    public bool Recommended { get; set; } = false;

    public int DurationDays { get; set; }

    public int DurationNights { get; set; }

    public string? BestTimeToVisit { get; set; }

    public Guid CategoryId { get; set; }

    public byte[]? RowVersion { get; set; }
    public DateTime? LastReservedAt { get; set; }



    // Navigation Properties

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<TourImage> Images { get; set; } = new List<TourImage>();

    public virtual ICollection<TourActivity> Activities { get; set; } = new List<TourActivity>();

    public virtual ICollection<TourPriceTier> PriceTiers { get; set; } = new List<TourPriceTier>();

    public virtual ICollection<OnlineTravel.Domain.Entities.Reviews.Review> Reviews { get; set; } = new List<OnlineTravel.Domain.Entities.Reviews.Review>();



    public void UpdateAddress(Address newAddress)
    {
        Address = newAddress;
    }
}




