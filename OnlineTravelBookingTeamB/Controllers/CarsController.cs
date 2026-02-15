using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Cars.Commands;
using OnlineTravel.Application.Features.Cars.DTOs;
using OnlineTravel.Application.Features.Cars.Queries;
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

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            var query = new GetCarByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
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
            var query = new GetCarDetailsByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result.IsSuccess)
                return Ok(result.Value); // 200 + DTO

            // لو فشل، حوّل الـ Error إلى ProblemDetails (حسب ToProblem extension)
            return result.ToProblem();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateCarRequest request)
        {
            var command = new CreateCarCommand { Data = request };
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, UpdateCarRequest request)
        {
            if (id != request.Id)
                return BadRequest("ID mismatch");

            var command = new UpdateCarCommand { Data = request };
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeleteCarCommand { Id = id };
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }
    }
}
