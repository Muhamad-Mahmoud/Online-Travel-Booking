using Domain.ErrorHandling;
using MediatR;
using OnlineTravel.Application.Features.CarBrands.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.CarBrands.Commands
{
    public record UpdateCarBrandCommand(Guid Id, UpdateCarBrandDto Dto) : IRequest<Result<CarBrandDto>>;
}
