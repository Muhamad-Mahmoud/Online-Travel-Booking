using MediatR;
using OnlineTravel.Application.Features.CarPricingTiers.DTOs;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.CarPricingTiers.Queries
{
    public class GetAllCarPricingTiersQuery : IRequest<Result<IReadOnlyList<CarPricingTierDto>>>
    {
        public Guid? CarId { get; set; }
    }

}
