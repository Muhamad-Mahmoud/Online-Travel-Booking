using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace OnlineTravelBookingTeamB.Models;

// ───────── Create Hotel ─────────
public class CreateHotelViewModel
{
    [Required(ErrorMessage = "Hotel name is required")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "URL slug is required")]
    public string Slug { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required")]
    public string Description { get; set; } = string.Empty;

    // Address
    public string Street { get; set; } = string.Empty;

    [Required(ErrorMessage = "City is required")]
    public string City { get; set; } = string.Empty;
    public string? State { get; set; }

    [Required(ErrorMessage = "Country is required")]
    public string Country { get; set; } = string.Empty;
    public string? PostalCode { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }

    // Check-in/out times (Start → End range)
    [Required] public string CheckInTimeStart { get; set; } = "14:00";
    [Required] public string CheckInTimeEnd { get; set; } = "16:00";
    [Required] public string CheckOutTimeStart { get; set; } = "10:00";
    [Required] public string CheckOutTimeEnd { get; set; } = "12:00";

    // Policy
    [Required(ErrorMessage = "Cancellation policy is required")]
    public string CancellationPolicy { get; set; } = string.Empty;

    // Contact
    public string? ContactPhone { get; set; }
    [EmailAddress] public string? ContactEmail { get; set; }
    [Url] public string? Website { get; set; }

    // Image upload
    public IFormFile? MainImage { get; set; }
}

// ───────── Edit Hotel ─────────
public class EditHotelViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Hotel name is required")]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    // Address
    public string Street { get; set; } = string.Empty;

    [Required(ErrorMessage = "City is required")]
    public string City { get; set; } = string.Empty;
    public string? State { get; set; }

    [Required(ErrorMessage = "Country is required")]
    public string Country { get; set; } = string.Empty;
    public string? PostalCode { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    // Check-in/out (single time for Update command)
    public string CheckInTime { get; set; } = "14:00";
    public string CheckOutTime { get; set; } = "10:00";

    // Policy
    [Required(ErrorMessage = "Cancellation policy is required")]
    public string CancellationPolicy { get; set; } = string.Empty;

    // Contact
    public string? ContactPhone { get; set; }
    [EmailAddress] public string? ContactEmail { get; set; }
    public string? Website { get; set; }

    // Image
    public string? MainImage { get; set; }
    public string? CurrentImageUrl { get; set; }
}

// ───────── Manage Hotel (Edit + Add Room + Rooms List) ─────────
public class ManageHotelViewModel
{
    public OnlineTravel.Domain.Entities.Hotels.Hotel Hotel { get; set; } = null!;
    public EditHotelViewModel EditForm { get; set; } = new();
    public AddRoomViewModel RoomForm { get; set; } = new();
}

// ───────── Add Room ─────────
public class AddRoomViewModel
{
    public Guid HotelId { get; set; }

    [Required(ErrorMessage = "Room number is required")]
    public string RoomNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Room name is required")]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(0.01, 99999, ErrorMessage = "Price must be greater than 0")]
    public decimal BasePricePerNight { get; set; }
}

// ───────── Edit Room ─────────
public class EditRoomViewModel
{
    public Guid Id { get; set; }
    public Guid HotelId { get; set; }
    [Required(ErrorMessage = "Room name is required")]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(0.01, 99999, ErrorMessage = "Price must be greater than 0")]
    public decimal BasePricePerNight { get; set; }
}
