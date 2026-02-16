using OnlineTravel.Application.Features.Tours.GetTourById.DTOs;
using OnlineTravel.Application.Features.Tours.Manage.Commands.AddActivity;
using OnlineTravel.Application.Features.Tours.Manage.Commands.AddImage;
using OnlineTravel.Application.Features.Tours.Manage.Commands.AddPriceTier;

namespace OnlineTravelBookingTeamB.Models;

    public class ManageTourViewModel
    {
        public TourDetailsResponse Tour { get; set; } = null!;
        
        public AddActivityViewModel ActivityForm { get; set; } = new();
        public AddTourImageViewModel ImageForm { get; set; } = new();
        public AddTourPriceTierCommand PriceTierForm { get; set; } = new();
        public UpdateCoordinatesViewModel LocationForm { get; set; } = new();
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
