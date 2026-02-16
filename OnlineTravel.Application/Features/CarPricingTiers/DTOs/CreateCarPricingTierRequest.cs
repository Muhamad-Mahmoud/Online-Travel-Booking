using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.CarPricingTiers.DTOs
{
    public class CreateCarPricingTierRequest
    {
        [Required]
        public Guid CarId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "FromHours must be >= 0")]
        public int FromHours { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "ToHours must be > 0")]
        public int ToHours { get; set; }

        [Required]
        public MoneyDto PricePerHour { get; set; } = null!;
    }
}
