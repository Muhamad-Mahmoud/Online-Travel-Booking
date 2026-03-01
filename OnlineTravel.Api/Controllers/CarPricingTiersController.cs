using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.CarPricingTiers.CreateCarPricingTiers;
using OnlineTravel.Application.Features.CarPricingTiers.Delete;
using OnlineTravel.Application.Features.CarPricingTiers.GetAllCarPricingTiers;
using OnlineTravel.Application.Features.CarPricingTiers.GetCarPricingTiersById;
using OnlineTravel.Application.Features.CarPricingTiers.UpdateCarPricingTier;

namespace OnlineTravel.Api.Controllers;

[Route("api/v1/car-pricing-tiers")]
[Authorize(Roles = "Admin")]
public class CarPricingTiersController : BaseApiController
{
	/// <summary>
	/// List pricing tiers for cars (Admin only).
	/// </summary>
	[HttpGet]
	public async Task<ActionResult> GetAll([FromQuery] Guid? carId = null)
	{
		var result = await Mediator.Send(new GetAllCarPricingTiersQuery(carId));
		return HandleResult(result);
	}

	/// <summary>
	/// Get a specific car pricing tier by ID (Admin only).
	/// </summary>
	[HttpGet("{id:guid}")]
	public async Task<ActionResult> GetById(Guid id)
	{
		var result = await Mediator.Send(new GetCarPricingTierByIdQuery(id));
		return HandleResult(result);
	}

	/// <summary>
	/// Create a new car pricing tier (Admin only).
	/// </summary>
	[HttpPost]
	public async Task<ActionResult> Create([FromBody] CreateCarPricingTierCommand command)
	{
		var result = await Mediator.Send(command);
		return HandleResult(result);
	}

	/// <summary>
	/// Update an existing car pricing tier (Admin only).
	/// </summary>
	[HttpPut("{id:guid}")]
	public async Task<ActionResult> Update(Guid id, [FromBody] UpdateCarPricingTierCommand command)
	{
		var payload = command with { Id = id };
		var result = await Mediator.Send(payload);
		return HandleResult(result);
	}

	/// <summary>
	/// Delete a car pricing tier (Admin only).
	/// </summary>
	[HttpDelete("{id:guid}")]
	public async Task<ActionResult> Delete(Guid id)
	{
		var result = await Mediator.Send(new DeleteCarPricingTierCommand(id));
		return HandleResult(result);
	}
}

