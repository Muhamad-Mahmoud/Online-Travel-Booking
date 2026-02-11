using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Bookings.Commands.CreateBooking;
using OnlineTravelBookingTeamB.Extensions;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravelBookingTeamB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateBookingCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ToResponse(201);
        }

    }
}
