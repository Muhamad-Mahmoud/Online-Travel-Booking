using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Flight.Airport.GetAirportById;
using OnlineTravel.Application.Features.Flight.Airport.GetAllAirports;
using OnlineTravel.Application.Features.Flight.Airport.UpdateAirport;
using OnlineTravelBookingTeamB.Extensions;
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
        public async Task<ActionResult> Create(CreateAirportCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<GetAirportByIdDto>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetAirportByIdQuery(id));
            if (result == null) return NotFound();
            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult<List<GetAllAirportsDto>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllAirportsQuery());
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateAirportResponse>> Update(Guid id, UpdateAirportCommand command)
        {
            if (id != command.Id) return BadRequest("ID mismatch");

            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
