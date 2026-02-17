using OnlineTravel.Application.Features.Tours.GetTourById.DTOs;
using OnlineTravel.Application.Features.Tours.Manage.Commands.AddActivity;
using OnlineTravel.Application.Features.Tours.Manage.Commands.AddImage;
using OnlineTravel.Application.Features.Tours.Manage.Commands.AddPriceTier;
using System.ComponentModel.DataAnnotations;

namespace OnlineTravelBookingTeamB.Models;

    public class ManageTourViewModel
    {
        public TourDetailsResponse Tour { get; set; } = null!;
        
        public EditTourViewModel EditForm { get; set; } = new();
        public AddActivityViewModel ActivityForm { get; set; } = new();
        public AddTourImageViewModel ImageForm { get; set; } = new();
        public AddTourPriceTierCommand PriceTierForm { get; set; } = new();
        public UpdateCoordinatesViewModel LocationForm { get; set; } = new();
    }

    public class EditTourViewModel
    {
        public Guid TourId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Category")]
        public Guid CategoryId { get; set; }

        [Required]
        [Range(1, 365)]
        [Display(Name = "Duration (Days)")]
        public int DurationDays { get; set; }

        [Required]
        [Range(0, 365)]
        [Display(Name = "Duration (Nights)")]
        public int DurationNights { get; set; }

        [Display(Name = "Best Time to Visit")]
        public string? BestTimeToVisit { get; set; }

        public bool Recommended { get; set; }

        // Address Fields
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }

        // Main Image (optional on update)
        [Display(Name = "Main Image")]
        public IFormFile? MainImage { get; set; }
    }
    
    public class UpdateCoordinatesViewModel
    {
        public Guid TourId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }

    public class AddActivityViewModel
    {
        public Guid TourId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IFormFile? Image { get; set; }
    }

    public class AddTourImageViewModel
    {
        public Guid TourId { get; set; }
        public IFormFile? Image { get; set; }
        public string? AltText { get; set; }
    }

