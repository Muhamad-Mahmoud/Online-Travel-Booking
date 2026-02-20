using Microsoft.AspNetCore.Http;

namespace OnlineTravelBookingTeamB.Models;

// Create New Hotel With Image
public class CreateHotelFormRequest
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }

    public string CheckInTimeStart { get; set; } = string.Empty;  // e.g. "14:00"
    public string CheckInTimeEnd { get; set; } = string.Empty;
    public string CheckOutTimeStart { get; set; } = string.Empty;
    public string CheckOutTimeEnd { get; set; } = string.Empty;

    public string CancellationPolicy { get; set; } = string.Empty;
    public string? ContactPhone { get; set; }
    public string? ContactEmail { get; set; }
    public string? Website { get; set; }

    public IFormFile? MainImage { get; set; }
}
