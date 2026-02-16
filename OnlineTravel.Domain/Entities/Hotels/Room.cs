using OnlineTravel.Domain.Entities._Base;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Bookings;
using OnlineTravel.Domain.Enums;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Domain.Entities.Hotels;

public class Room : BaseEntity
{
    public Guid HotelId { get; private set; }
    public string RoomNumber { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Money BasePricePerNight { get; private set; }
    public int Capacity { get; private set; }
    public int BedCount { get; private set; }

    private readonly List<Url> _photos = new();
    public IReadOnlyCollection<Url> Photos => _photos.AsReadOnly();

    // Navigation properties
    public Hotel Hotel { get; private set; }

    private readonly List<SeasonalPrice> _seasonalPrices = new();
    public IReadOnlyCollection<SeasonalPrice> SeasonalPrices => _seasonalPrices.AsReadOnly();

    private readonly List<RoomAvailability> _roomAvailabilities = new();
    public IReadOnlyCollection<RoomAvailability> RoomAvailabilities => _roomAvailabilities.AsReadOnly();

    private readonly List<BookingEntity> _bookings = new();
    public IReadOnlyCollection<BookingEntity> Bookings => _bookings.AsReadOnly();

    private Room() { } // EF Core

    public Room(
        Guid hotelId,
        string roomNumber,
        string name,
        string description,
        Money basePricePerNight)
    {
        if (hotelId == Guid.Empty)
            throw new ArgumentException("Hotel ID is required", nameof(hotelId));

        if (string.IsNullOrWhiteSpace(roomNumber))
            throw new ArgumentException("Room number is required", nameof(roomNumber));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required", nameof(name));

        if (basePricePerNight == null || basePricePerNight.Amount <= 0)
            throw new ArgumentException("Base price per night must be greater than zero", nameof(basePricePerNight));

        Id = Guid.NewGuid();
        HotelId = hotelId;
        RoomNumber = roomNumber;
        Name = name;
        Description = description;
        BasePricePerNight = basePricePerNight;
        Capacity = 1; // Fixed as per requirements
        BedCount = 1; // Fixed as per requirements
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string name, string description, Money basePricePerNight)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required", nameof(name));

        if (basePricePerNight == null || basePricePerNight.Amount <= 0)
            throw new ArgumentException("Base price per night must be greater than zero", nameof(basePricePerNight));

        Name = name;
        Description = description;
        BasePricePerNight = basePricePerNight;
    }

    public void AddPhoto(Url photoUrl)
    {
        if (photoUrl == null)
            throw new ArgumentNullException(nameof(photoUrl));

        _photos.Add(photoUrl);
    }

    public void RemovePhoto(Url photoUrl)
    {
        _photos.Remove(photoUrl);
    }

    public void AddSeasonalPrice(SeasonalPrice seasonalPrice)
    {
        if (seasonalPrice == null)
            throw new ArgumentNullException(nameof(seasonalPrice));

        // Check for overlapping date ranges
        if (_seasonalPrices.Any(sp => sp.DateRange.Overlaps(seasonalPrice.DateRange)))
            throw new InvalidOperationException("Seasonal price date range overlaps with existing seasonal price");

        _seasonalPrices.Add(seasonalPrice);
    }

    public void SetAvailability(RoomAvailability availability)
    {
        if (availability == null)
            throw new ArgumentNullException(nameof(availability));

        // Check for overlapping date ranges
        var overlapping = _roomAvailabilities
            .Where(a => a.DateRange.Overlaps(availability.DateRange))
            .ToList();

        foreach (var item in overlapping)
        {
            _roomAvailabilities.Remove(item);
        }

        _roomAvailabilities.Add(availability);
    }

    public void AddBooking(BookingEntity booking)
    {
        if (booking == null)
            throw new ArgumentNullException(nameof(booking));

        _bookings.Add(booking);
    }

    public Money GetPriceForDate(DateOnly date)
    {
        var seasonalPrice = _seasonalPrices
            .FirstOrDefault(sp => sp.DateRange.Contains(date));

        return seasonalPrice?.PricePerNight ?? BasePricePerNight;
    }

    public Money CalculateTotalPrice(DateRange dateRange)
    {
        if (dateRange == null)
            throw new ArgumentNullException(nameof(dateRange));

        var totalPrice = new Money(0, BasePricePerNight.Currency);

        foreach (var date in dateRange.GetDates())
        {
            var dateOnly = DateOnly.FromDateTime(date);
            var priceForDate = GetPriceForDate(dateOnly);
            totalPrice = totalPrice.Add(priceForDate);
        }

        return totalPrice;
    }

    public bool IsAvailable(DateRange dateRange)
    {
        if (dateRange == null)
            throw new ArgumentNullException(nameof(dateRange));

        // Check if there's any unavailability that overlaps
        var hasUnavailability = _roomAvailabilities
            .Where(a => !a.IsAvailable && a.DateRange.Overlaps(dateRange))
            .Any();

        if (hasUnavailability)
            return false;

        // Check for overlapping bookings
        var hasOverlappingBooking = _bookings
            .Where(b => b.Status != BookingStatus.Cancelled && b.DateRange.Overlaps(dateRange))
            .Any();

        return !hasOverlappingBooking;
    }
}