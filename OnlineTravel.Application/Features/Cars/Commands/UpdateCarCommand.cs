using MediatR;
using OnlineTravel.Application.Features.Cars.DTOs;
using OnlineTravel.Domain.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Cars.Commands
{
    public class UpdateCarCommand : IRequest<Result<Guid>>
    {
        public UpdateCarRequest Data { get; set; } = null!;
    }
}
