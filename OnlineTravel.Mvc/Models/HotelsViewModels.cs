using OnlineTravel.Application.Features.Hotels.Admin.CreateHotelCommand;
using OnlineTravel.Domain.Entities._Shared.ValueObjects;

namespace OnlineTravel.Mvc.Models;

public class HotelsCreateViewModel : CreateHotelCommand
{
    public IFormFile? ImageFile { get; set; }
}

public class HotelsEditViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? MainImage { get; set; }
    public IFormFile? ImageFile { get; set; }
    public string CurrentImageUrl { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public TimeSpan CheckInTime { get; set; }
    public TimeSpan CheckOutTime { get; set; }
    public string ContactPhone { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;
    public string CancellationPolicy { get; set; } = string.Empty;
}

public class HotelsManageViewModel
{
    public Hotel Hotel { get; set; } = new();
    public RoomFormDto RoomForm { get; set; } = new();
}

public class RoomFormDto
{
    public Guid Id { get; set; }
    public Guid HotelId { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public decimal BasePrice { get; set; }
}

public class Hotel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string MainImageUrl { get; set; } = string.Empty;
    public Address? Address { get; set; }
    public Rating? Rating { get; set; }
    public List<Room> Rooms { get; set; } = [];
    public string ContactEmail { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;
    public string CancellationPolicy { get; set; } = string.Empty;
    public TimeRange? CheckInTime { get; set; }
    public TimeRange? CheckOutTime { get; set; }
    public ContactInfo? ContactInfo { get; set; }
}

public class Address
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class Rating
{
    public decimal Value { get; set; }
}

public class Room
{
    public Guid Id { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Money BasePricePerNight { get; set; } = new(0);
}
