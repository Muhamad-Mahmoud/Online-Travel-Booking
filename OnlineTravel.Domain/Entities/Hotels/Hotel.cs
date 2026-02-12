using NetTopologySuite.Geometries;
using OnlineTravel.Domain.Entities._Base;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using OnlineTravel.Domain.Entities.Core;
using OnlineTravel.Domain.Entities.Reviews.ValueObjects;

namespace OnlineTravel.Domain.Entities.Hotels;

public class Hotel : SoftDeletableEntity
{
    /// <summary>
    /// Hotel name
    /// Example: "Grand Nile Hotel", "Hilton Cairo"
    /// Required field
    /// </summary>

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public Address Address { get; set; } = null!;
    public ContactInfo? ContactInfo { get; set; }

    public ImageUrl? MainImage { get; set; }

    public List<ImageUrl> Gallery { get; set; } = new();


    public StarRating? StarRating { get; set; }


    public Guid CategoryId { get; set; }

    // Navigation Properties

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();

    #region 
    // Add a new room to the hotel

     public void AddRoom(Room  room)
    {
        if (room == null)
        {
            throw new ArgumentNullException(nameof(room), "Room cannot be null");
        }
        room.HotelId = this.Id; // Set the foreign key to establish the relationship
        Rooms.Add(room);
        UpdatedAt = DateTime.UtcNow; // Update the timestamp for the hotel entity
    }

    // Add Image to gallery 
    public void AddGalleryImage(ImageUrl imageUrl)
    {
        if(imageUrl == null)
        {
              throw new ArgumentNullException(nameof(imageUrl), "Image URL cannot be null");
        }
        Gallery.Add(imageUrl);
        UpdatedAt = DateTime.UtcNow; // Update the timestamp for the hotel entity
    }

    /// <summary>
    /// Calculate distance to another hotel in kilometers
    /// Uses NetTopologySuite Point.Distance()
    /// </summary>
    /// <param name="otherHotel">Hotel to calculate distance to</param>
    /// <returns>Distance in kilometers, or null if coordinates missing</returns>
    public double? DistanceToInKm(Hotel otherHotel)
    {
        return Address?.DistanceToInKm(otherHotel.Address);
    }


    #endregion
}





