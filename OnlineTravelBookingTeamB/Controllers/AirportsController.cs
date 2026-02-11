using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Flight.CreateAirport;

namespace OnlineTravelBookingTeamB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AirportsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<CreateAirportResponse>> Create(CreateAirportCommand command)
        {
           
            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}
