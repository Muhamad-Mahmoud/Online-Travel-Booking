using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.CarPricingTiers.DTOs
{
    public class UpdateCarPricingTierRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid CarId { get; set; }

        [Range(0, int.MaxValue)]
        public int FromHours { get; set; }

        [Range(1, int.MaxValue)]
        public int ToHours { get; set; }

        [Required]
        public MoneyDto PricePerHour { get; set; } = null!;
    }

}
