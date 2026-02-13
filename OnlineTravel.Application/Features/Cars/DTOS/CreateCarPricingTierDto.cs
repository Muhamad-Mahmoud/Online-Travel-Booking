using OnlineTravel.Domain.Entities._Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Cars.DTOS
{
    public class CreateCarPricingTierDto
    {
        public int FromHours { get; set; }
        public int ToHours { get; set; }
        public Money PricePerHour { get; set; }
    }
}
