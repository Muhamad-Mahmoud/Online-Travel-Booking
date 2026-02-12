using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Flight.CreateAirport;
using OnlineTravelBookingTeamB.Extensions;
using OnlineTravel.Domain.ErrorHandling;

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
        public async Task<ActionResult> Create(CreateAirportCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ToResponse();
        }
    }
}
