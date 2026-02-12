using OnlineTravel.Domain.Entities._Base;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;

namespace OnlineTravel.Domain.Entities.Hotels;

public class Room : BaseEntity
{
    public Guid HotelId { get; set; }

    public string RoomNumber { get; set; } = string.Empty;

    public string RoomType { get; set; } = string.Empty; // Single, Double, Suite

    public Money PricePerNight { get; private set; }

    public List<DateRange> AvailableDates { get; set; } = new();
    
  //  public List<string> Extras { get; set; } = new();

    public int? MinimumStayNights { get; set; }

    public bool IsAvailable { get; private set; }
    public List<string> Extras { get; set; } = new();


    // Navigation Properties

    public virtual Hotel Hotel { get; set; } = null!;

    public void UpdatePrice(decimal amount, string currency = "USD")
    {
        PricePerNight = new Money(amount, currency);
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetAvailability(bool isAvailable)
    {
        IsAvailable = isAvailable;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Add a date range to available dates
    /// </summary>
    /// <param name="dateRange">Date range to add</param>
    public void AddAvailableDateRange(DateRange dateRange)
    {
        if (dateRange == null)
            throw new ArgumentNullException(nameof(dateRange));

        // Check for overlaps with existing ranges
        if (AvailableDates.Any(dr => dr.OverlapsWith(dateRange)))
            throw new InvalidOperationException("Date range overlaps with existing availability");

        AvailableDates.Add(dateRange);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Check if room is available for a specific date range
    /// </summary>
    /// <param name="requestedRange">Date range to check</param>
    /// <returns>True if room is available for entire range</returns>
    public bool IsAvailableForDates(DateRange requestedRange)
    {
        if (!IsAvailable)
            return false;

        if (requestedRange == null)
            return false;

        // Check if requested range falls within any available date range
        return AvailableDates.Any(availableRange =>
            requestedRange.Start >= availableRange.Start &&
            requestedRange.End <= availableRange.End);
    }

    /// <summary>
    /// Calculate total price for a stay
    /// </summary>
    /// <param name="dateRange">Stay date range</param>
    /// <returns>Total price for the stay</returns>
    public Money CalculateTotalPrice(DateRange dateRange)
    {
        if (dateRange == null)
            throw new ArgumentNullException(nameof(dateRange));

        var nights = dateRange.TotalNights;
        var totalAmount = PricePerNight.Amount * nights;

        return new Money(totalAmount, PricePerNight.Currency);
    }

}