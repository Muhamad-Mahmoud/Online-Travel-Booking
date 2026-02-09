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

    public Guid CategoryId { get; set; }

    public byte[]? RowVersion { get; set; }

    // Navigation Properties

    public virtual Category Category { get; set; } = null!;

    public List<ImageUrl> Images { get; set; } = new();

    public virtual ICollection<TourPriceTier> PriceTiers { get; set; } = new List<TourPriceTier>();
}




