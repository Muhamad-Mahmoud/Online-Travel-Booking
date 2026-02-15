using System.ComponentModel.DataAnnotations;

namespace OnlineTravelBookingTeamB.Models
{
    public class TourViewModel
    {
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

        // Main Image
        [Display(Name = "Main Image")]
        public IFormFile? MainImage { get; set; }

        // Price Tier (Simplification for creation: just one standard tier initially)
        [Required]
        [Display(Name = "Standard Price")]
        public decimal StandardPrice { get; set; }

        [Required]
        [Display(Name = "Currency")]
        public string Currency { get; set; } = "USD";
    }
}
