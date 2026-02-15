using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.CarPricingTiers.Commands;
using OnlineTravel.Application.Features.CarPricingTiers.DTOs;
using OnlineTravel.Application.Features.CarPricingTiers.Queries;
using OnlineTravelBookingTeamB.Extensions;

namespace OnlineTravelBookingTeamB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarPricingTiersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CarPricingTiersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            var query = new GetCarPricingTierByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] GetAllCarPricingTiersQuery query)
        {
            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateCarPricingTierRequest request)
        {
            var command = new CreateCarPricingTierCommand { Data = request };
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem(); 
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, UpdateCarPricingTierRequest request)
        {
            if (id != request.Id)
                return BadRequest("ID mismatch");

            var command = new UpdateCarPricingTierCommand { Data = request };
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem(); 
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeleteCarPricingTierCommand { Id = id };
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : result.ToProblem(); 
        }
    }
}
