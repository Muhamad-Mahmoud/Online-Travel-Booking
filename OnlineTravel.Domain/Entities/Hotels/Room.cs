using OnlineTravel.Domain.Entities._Base;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;

namespace OnlineTravel.Domain.Entities.Hotels;

public class Room : BaseEntity
{
    public Guid HotelId { get; set; }

    public string RoomNumber { get; set; } = string.Empty;

    public string RoomType { get; set; } = string.Empty; // Single, Double, Suite

    public Money BasePrice { get; set; } = null!;

    public int MaxGuests { get; set; } = 2;

    public List<DateRange> AvailableDates { get; set; } = new();

    public bool Refundable { get; set; } = false;

    public List<string> Extras { get; set; } = new();

    public int? MinimumStayNights { get; set; }

    public bool IsAvailable { get; set; } = true;

    // Navigation Properties

    public virtual Hotel Hotel { get; set; } = null!;
}