using Domain.ErrorHandling;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.CarBrands.Commands
{
    public record DeleteCarBrandCommand(Guid Id) : IRequest<Result<bool>>;
}
