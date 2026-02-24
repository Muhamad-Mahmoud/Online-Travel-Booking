using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Cars.CreateCar;
using OnlineTravel.Application.Features.Cars.GetCarById;
using OnlineTravel.Application.Features.Cars.GetAllCars;
using OnlineTravel.Application.Features.Cars.GetAllCarsSummary;
using OnlineTravel.Application.Features.Cars.GetCarByIdWithDetails;
using OnlineTravel.Application.Features.Cars.UpdateCar;
using OnlineTravel.Application.Features.Cars.DeleteCar;
using OnlineTravelBookingTeamB.Extensions;

namespace OnlineTravelBookingTeamB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CarsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] GetAllCarsQuery query)
        {
            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpGet("summary")]
        public async Task<ActionResult> GetAllSummary([FromQuery] GetAllCarsSummaryQuery query)
        {
            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("{id}/details")]
        public async Task<ActionResult<CarDetailsDto>> GetByIdWithDetails(Guid id)
        {
            var query = new GetCarDetailsByIdQuery(id);
            var result = await _mediator.Send(query);

            if (result.IsSuccess)
                return Ok(result.Value); 

            return result.ToProblem();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateCarRequest request)
        {
            var command = new CreateCarCommand(request);
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id,[FromBody] UpdateCarRequest request)
        {
            if (id != request.Id)
                return BadRequest("ID mismatch");

            var command = new UpdateCarCommand(request);
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeleteCarCommand(id);
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }
    }
}
