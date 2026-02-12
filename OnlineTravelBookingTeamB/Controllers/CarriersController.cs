using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Flight.Carrier.CreateCarrier;
using OnlineTravel.Application.Features.Flight.Carrier.GetCarrierById;
using OnlineTravel.Application.Features.Flight.Flights.CreateFlight;

namespace OnlineTravelBookingTeamB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarriersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CarriersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(CreateCarrierCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<GetCarrierByIdDto>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetCarrierByIdQuery(id));
            return result != null ? Ok(result) : NotFound();
        }
    }
}
