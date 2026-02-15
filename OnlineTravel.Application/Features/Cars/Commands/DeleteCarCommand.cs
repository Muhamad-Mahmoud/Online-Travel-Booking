using MediatR;
using OnlineTravel.Domain.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Application.Features.Cars.Commands
{
    public class DeleteCarCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}
