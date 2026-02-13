using Domain.ErrorHandling;
using MediatR;
using OnlineTravel.Application.Features.CarBrands.DTOs;
using OnlineTravel.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.CarBrands.Queries
{
    public record GetCarBrandsPaginatedQuery(int PageIndex, int PageSize, string? SearchTerm = null)
        : IRequest<Result<PaginatedResult<CarBrandDto>>>;
}
