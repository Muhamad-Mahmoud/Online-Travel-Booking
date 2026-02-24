using System.ComponentModel.DataAnnotations;

namespace OnlineTravel.Application.Features.CarBrands.CreateCarBrand
{
    public class CreateCarBrandRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
        
        public string? Logo { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
