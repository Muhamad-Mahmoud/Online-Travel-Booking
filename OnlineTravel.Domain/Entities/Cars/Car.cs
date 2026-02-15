using NetTopologySuite.Geometries;
using OnlineTravel.Domain.Entities._Base;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Core;
using OnlineTravel.Domain.Enums;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Domain.Entities.Cars;

public class Car : SoftDeletableEntity
{
    private Category category = null!;

    public Guid BrandId { get; set; }

    public string Make { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public CarCategory CarType { get; set; }

    public byte[]? RowVersion { get; set; }
    public DateTime? LastReservedAt { get; set; }

    public void Reserve()
    {
        LastReservedAt = DateTime.UtcNow;
    }

    public int SeatsCount { get; set; }

    public FuelType FuelType { get; set; }

    public TransmissionType Transmission { get; set; }

    public List<string> Features { get; set; } = new();

    public List<DateTimeRange> AvailableDates { get; set; } = new();

    public string? CancellationPolicy { get; set; }

    public Guid CategoryId { get; set; }


    public Point Location { get; set; } = null!;


    // Navigation Properties

    public virtual CarBrand Brand { get; set; } = null!;

    public virtual Category Category { get => category; set => category = value; }
    public virtual ICollection<CarPricingTier> PricingTiers { get; set; } = new List<CarPricingTier>();

    public List<ImageUrl> Images { get; set; } = new();


}




