using MediatR;
using OnlineTravel.Application.Features.Cars.DTOs;
using OnlineTravel.Domain.Enums;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Cars.Queries
{
    public class GetAllCarsQuery : IRequest<Result<PaginatedResult<CarDto>>>
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public Guid? BrandId { get; set; }
        public Guid? CategoryId { get; set; }
        public CarCategory? CarType { get; set; }
    }
}
