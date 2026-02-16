using NetTopologySuite.Geometries;
using OnlineTravel.Domain.Entities._Base;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Reviews;
using OnlineTravel.Domain.Entities.Reviews.ValueObjects;

namespace OnlineTravel.Domain.Entities.Hotels;

public class Hotel : SoftDeletableEntity
{
    public string Name { get; private set; }
    public string Slug { get; private set; }
    public string Description { get; private set; }
    public Address Address { get; private set; }
    public ContactInfo ContactInfo { get; private set; }
    public string? MainImageUrl { get; private set; }
    public StarRating? Rating { get; private set; }
    public TimeRange CheckInTime { get; private set; }
    public TimeRange CheckOutTime { get; private set; }
    public string CancellationPolicy { get; private set; }


    // Navigation properties
    private readonly List<Room> _rooms = new();
    public IReadOnlyCollection<Room> Rooms => _rooms.AsReadOnly();

    private readonly List<Review> _reviews = new();
    public IReadOnlyCollection<Review> Reviews => _reviews.AsReadOnly();

    private Hotel() { } // EF Core

    public Hotel(
        string name,
        string slug,
        string description,
        Address address,
        ContactInfo contactInfo,
        TimeRange checkInTime,
        TimeRange checkOutTime,
        string cancellationPolicy,
        string? mainImageUrl = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required", nameof(name));

        if (string.IsNullOrWhiteSpace(slug))
            throw new ArgumentException("Slug is required", nameof(slug));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required", nameof(description));

        if (string.IsNullOrWhiteSpace(cancellationPolicy))
            throw new ArgumentException("Cancellation policy is required", nameof(cancellationPolicy));

        Id = Guid.NewGuid();
        Name = name;
        Slug = slug.ToLowerInvariant();
        Description = description;
        Address = address ?? throw new ArgumentNullException(nameof(address));
        ContactInfo = contactInfo ?? throw new ArgumentNullException(nameof(contactInfo));
        CheckInTime = checkInTime ?? throw new ArgumentNullException(nameof(checkInTime));
        CheckOutTime = checkOutTime ?? throw new ArgumentNullException(nameof(checkOutTime));
        CancellationPolicy = cancellationPolicy;
        MainImageUrl = mainImageUrl;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string name, string description, string cancellationPolicy)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required", nameof(name));

        Name = name;
        Description = description;
        CancellationPolicy = cancellationPolicy;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateAddress(Address address)
    {
        Address = address ?? throw new ArgumentNullException(nameof(address));
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateContactInfo(ContactInfo contactInfo)
    {
        ContactInfo = contactInfo ?? throw new ArgumentNullException(nameof(contactInfo));
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateCheckInCheckOut(TimeRange checkInTime, TimeRange checkOutTime)
    {
        CheckInTime = checkInTime ?? throw new ArgumentNullException(nameof(checkInTime));
        CheckOutTime = checkOutTime ?? throw new ArgumentNullException(nameof(checkOutTime));
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetMainImage(string imageUrl)
    {
        MainImageUrl = imageUrl;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddRoom(Room room)
    {
        if (room == null)
            throw new ArgumentNullException(nameof(room));

        if (_rooms.Any(r => r.RoomNumber == room.RoomNumber))
            throw new InvalidOperationException($"Room with number {room.RoomNumber} already exists");

        _rooms.Add(room);
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddReview(Review review)
    {
        if (review == null)
            throw new ArgumentNullException(nameof(review));

        _reviews.Add(review);
        RecalculateRating();
        UpdatedAt = DateTime.UtcNow;
    }

    private void RecalculateRating()
    {
        if (_reviews.Count == 0)
        {
            Rating = null;
            return;
        }

        var averageRating = _reviews.Average(r => (int)r.Rating);
        Rating = new StarRating((int)Math.Round(averageRating));
    }
}





