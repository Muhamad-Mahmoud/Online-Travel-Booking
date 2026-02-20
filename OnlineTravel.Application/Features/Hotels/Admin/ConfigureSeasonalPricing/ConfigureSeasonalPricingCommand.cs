using MediatR;
using OnlineTravel.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Hotels.Admin.ConfigureSeasonalPricing
{
    public class ConfigureSeasonalPricingCommand : IRequest<Result<ConfigureSeasonalPricingResponse>>
    {
        public Guid RoomId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public decimal Price { get; set; }
    }

}
