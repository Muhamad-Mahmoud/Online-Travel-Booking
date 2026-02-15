using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.CarPricingTiers.DTOs
{
    public class CarPricingTierDto
    {
        public Guid Id { get; set; }
        public Guid CarId { get; set; }
        public int FromHours { get; set; }
        public int ToHours { get; set; }
        public MoneyDto PricePerHour { get; set; } = null!;
    }
}
