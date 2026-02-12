using Microsoft.AspNetCore.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Flight.Flights.CreateFlight;

namespace OnlineTravelBookingTeamB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly IMediator _mediator;
      public  FlightsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(CreateFlightCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
