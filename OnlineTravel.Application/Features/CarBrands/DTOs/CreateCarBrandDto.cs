using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.CarBrands.DTOs
{
    public class CreateCarBrandDto
    {
        [Required(ErrorMessage = "Car brand name is required")]
        [StringLength(100, ErrorMessage = "Car brand name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "Logo path cannot exceed 255 characters")]
        public string? Logo { get; set; }

        [Required(ErrorMessage = "IsActive must be specified")]
        public bool IsActive { get; set; } = true;
    }
}
