using OnlineTravel.Domain.Entities._Base;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Enums;

namespace OnlineTravel.Domain.Entities.Hotels;

public class Room : BaseEntity
{
    public Guid HotelId { get; set; }

    public string RoomNumber { get; set; } = string.Empty;

    public string RoomType { get; set; } = string.Empty; // Single, Double, Suite

    public Money BasePrice { get; set; } = null!;

    public int MaxGuests { get; set; } = 2;

    public RoomStatus Status { get; set; } = RoomStatus.Active;

    public bool Refundable { get; set; } = false;

    public List<string> Extras { get; set; } = new();

    public int? MinimumStayNights { get; set; }

    // Navigation Properties

    public virtual Hotel Hotel { get; set; } = null!;

    public bool IsBookable(DateRange stayRange, IEnumerable<DateRange> conflictingSlots)
    {
        if (Status != RoomStatus.Active)
            return false;

        if (MinimumStayNights.HasValue && stayRange.TotalNights < MinimumStayNights.Value)
            return false;

        // check against conflicting slots
        // conflictingSlots are assumed to be ranges that CANNOT coexist with new booking
        // usually fetched from existing bookings
        foreach (var slot in conflictingSlots)
        {
            if (stayRange.OverlapsWith(slot))
                return false;
        }

        return true;
    }
}
