using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.CarPricingTiers.Create;
using OnlineTravel.Application.Features.CarPricingTiers.Delete;
using OnlineTravel.Application.Features.CarPricingTiers.GetAll;
using OnlineTravel.Application.Features.CarPricingTiers.GetById;
using OnlineTravel.Application.Features.CarPricingTiers.Update;

namespace OnlineTravel.Api.Controllers;

[Route("api/v1/car-pricing-tiers")]
[Authorize(Roles = "Admin")]
public class CarPricingTiersController : BaseApiController
{
	[HttpGet]
	public async Task<ActionResult> GetAll([FromQuery] Guid? carId = null)
	{
		var result = await Mediator.Send(new GetAllCarPricingTiersQuery(carId));
		return HandleResult(result);
	}

	[HttpGet("{id:guid}")]
	public async Task<ActionResult> GetById(Guid id)
	{
		var result = await Mediator.Send(new GetCarPricingTierByIdQuery(id));
		return HandleResult(result);
	}

	[HttpPost]
	public async Task<ActionResult> Create([FromBody] CreateCarPricingTierCommand command)
	{
		var result = await Mediator.Send(command);
		return HandleResult(result);
	}

	[HttpPut("{id:guid}")]
	public async Task<ActionResult> Update(Guid id, [FromBody] UpdateCarPricingTierCommand command)
	{
		var payload = command with { Id = id };
		var result = await Mediator.Send(payload);
		return HandleResult(result);
	}

	[HttpDelete("{id:guid}")]
	public async Task<ActionResult> Delete(Guid id)
	{
		var result = await Mediator.Send(new DeleteCarPricingTierCommand(id));
		return HandleResult(result);
	}
}

