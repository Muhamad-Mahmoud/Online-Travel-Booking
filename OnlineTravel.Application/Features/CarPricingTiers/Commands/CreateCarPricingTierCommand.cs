using MediatR;

using OnlineTravel.Application.Features.CarPricingTiers.DTOs;
using OnlineTravel.Domain.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.CarPricingTiers.Commands
{
    public class CreateCarPricingTierCommand : IRequest<Result<Guid>>
    {
        public CreateCarPricingTierRequest Data { get; set; } = null!;
    }


}
