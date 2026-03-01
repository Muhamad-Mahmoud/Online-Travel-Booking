using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Flights.Carrier.CreateCarrier;
using OnlineTravel.Application.Features.Flights.Carrier.GetCarrierById;
using OnlineTravel.Application.Features.Flights.Carrier.GetAllCarriers;
using OnlineTravel.Application.Features.Flights.Carriers.DeleteCarrier;

namespace OnlineTravel.Api.Controllers;

[Route("api/v1/flights/carriers")]
public class CarriersController : BaseApiController
{
	[Authorize(Roles = "Admin")]
	[HttpPost]
	public async Task<ActionResult> Create([FromBody] CreateCarrierCommand command)
	{
		var result = await Mediator.Send(command);
		if (!result.IsSuccess)
			return HandleResult(result);

		return CreatedAtAction(nameof(GetById), new { id = result.Value }, new { id = result.Value });
	}

	[Authorize(Roles = "Admin")]
	[HttpGet("{id:guid}")]
	public async Task<ActionResult> GetById(Guid id)
	{
		var result = await Mediator.Send(new GetCarrierByIdQuery(id));
		return HandleResult(result);
	}

	[Authorize(Roles = "Admin")]
	[HttpGet]
	public async Task<ActionResult> GetAll()
	{
		var result = await Mediator.Send(new GetAllCarriersQuery());
		return HandleResult(result);
	}

	[Authorize(Roles = "Admin")]
	[HttpDelete("{id:guid}")]
	public async Task<ActionResult> Delete(Guid id)
	{
		var result = await Mediator.Send(new DeleteCarrierCommand(id));
		return HandleResult(result);
	}
}

