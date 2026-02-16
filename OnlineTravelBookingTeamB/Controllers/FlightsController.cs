using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Flight.Flights.CreateFlight;
using OnlineTravel.Application.Features.Flight.Flights.SearchFlights;

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
        [HttpGet("search")]
        public async Task<ActionResult<List<SearchFlightsDto>>> Search([FromQuery] SearchFlightsQuery query)
        {
           
            var result = await _mediator.Send(query);

            if (result == null || !result.Any())
            {
                return NotFound("No flights found matching your criteria.");
            }

            return Ok(result);
        }
    }
}
