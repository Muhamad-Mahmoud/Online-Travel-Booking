using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.CarBrands.DTOs
{
    public class CarBrandDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Logo { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
