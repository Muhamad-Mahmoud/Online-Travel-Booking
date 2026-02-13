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
    public record CreateCarBrandCommand(CreateCarBrandDto Dto) : IRequest<Result<CarBrandDto>>;
}
