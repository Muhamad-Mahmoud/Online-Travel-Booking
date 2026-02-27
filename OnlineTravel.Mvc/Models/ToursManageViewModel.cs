using Microsoft.AspNetCore.Http;
using OnlineTravel.Application.Features.Tours.CreateTour;

namespace OnlineTravel.Mvc.Models;

public class TourCreateViewModel : CreateTourCommand
{
    public IFormFile? ImageFile { get; set; }
}

public class ToursManageViewModel
{
    public TourDto Tour { get; set; } = new();
    public TourEditFormDto EditForm { get; set; } = new();
    public ActivityFormDto ActivityForm { get; set; } = new();
    public PriceTierFormDto PriceTierForm { get; set; } = new();
}

public class PriceTierDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Capacity { get; set; }
}

public class ActivityDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
}

public class LocationDto
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}

public class TourDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DurationDays { get; set; }
    public int DurationNights { get; set; }
    public List<ActivityDto> Activities { get; set; } = new();
    public List<PriceTierDto> PriceTiers { get; set; } = new();
    public List<ImageDto> Images { get; set; } = new();
    public decimal Rating { get; set; }
    public LocationDto? Location { get; set; }
    public string Category { get; set; } = string.Empty;
    public bool Recommended { get; set; }
    public string MainImageUrl { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}

public class ImageDto
{
    public string Url { get; set; } = string.Empty;
    public string AltText { get; set; } = string.Empty;
}

public class TourEditFormDto
{
    public Guid Id { get; set; }
    public Guid TourId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DurationDays { get; set; }
    public int DurationNights { get; set; }
    public string? CurrentImageUrl { get; set; }
    public string? MainImageUrl { get; set; }
    public IFormFile? ImageFile { get; set; }
}

public class ActivityFormDto
{
    public Guid Id { get; set; }
    public Guid TourId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Image { get; set; }
    public IFormFile? ImageFile { get; set; }
}

public class PriceTierFormDto
{
    public Guid Id { get; set; }
    public Guid TourId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal Amount { get; set; }
    public int Capacity { get; set; }
}
