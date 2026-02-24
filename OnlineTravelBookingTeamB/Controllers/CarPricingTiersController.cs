using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.CarPricingTiers.Create;
using OnlineTravelBookingTeamB.Extensions;
using OnlineTravel.Application.Features.CarPricingTiers.Update;
using OnlineTravel.Application.Features.CarPricingTiers.GetById;
using OnlineTravel.Application.Features.CarPricingTiers.GetAll;
using MoneyCommand = OnlineTravel.Application.Features.CarPricingTiers.Common.MoneyCommand;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using OnlineTravel.Application.Features.CarPricingTiers.Delete;

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
            var query = new GetCarPricingTierByIdQuery(id);
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
        public async Task<ActionResult> Create(CreateCarPricingTierFormModel formModel)
        {
            var command = new CreateCarPricingTierCommand(
                formModel.CarId,
                formModel.FromHours,
                formModel.ToHours,
				new MoneyCommand(formModel.PricePerHour.Amount, formModel.PricePerHour.Currency));

            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem(); 
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, UpdateCarPricingTierFormModel formModel)
        {
            if (id != formModel.Id)
                return BadRequest("ID mismatch");

            var command = new UpdateCarPricingTierCommand(
                formModel.Id,
                formModel.CarId,
                formModel.FromHours,
                formModel.ToHours,
                new MoneyCommand(formModel.PricePerHour.Amount, formModel.PricePerHour.Currency));

            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem(); 
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeleteCarPricingTierCommand(id);
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : result.ToProblem(); 
        }
    }
}
