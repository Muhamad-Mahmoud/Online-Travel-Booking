using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Flight.Airport.GetAirportById;
using OnlineTravel.Application.Features.Flight.Airport.GetAllAirports;
using OnlineTravel.Application.Features.Flight.Airport.UpdateAirport;
using OnlineTravel.Application.Features.Flight.CreateAirport;

namespace OnlineTravel.Api.Controllers;

[Route("api/v1/flights/airports")]
public class AirportsController : BaseApiController
{
	[Authorize(Roles = "Admin")]
	[HttpGet]
	public async Task<ActionResult> GetAll([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 100)
	{
		if (pageIndex <= 0 || pageSize <= 0)
			return BadRequest("pageIndex and pageSize must be greater than 0.");

		var result = await Mediator.Send(new GetAllAirportsQuery { PageIndex = pageIndex, PageSize = pageSize });
		return HandleResult(result);
	}

	[Authorize(Roles = "Admin")]
	[HttpGet("{id:guid}")]
	public async Task<ActionResult> GetById(Guid id)
	{
		var result = await Mediator.Send(new GetAirportByIdQuery(id));
		return HandleResult(result);
	}

	[Authorize(Roles = "Admin")]
	[HttpPost]
	public async Task<ActionResult> Create([FromBody] CreateAirportCommand command)
	{
		var result = await Mediator.Send(command);
		if (!result.IsSuccess)
			return HandleResult(result);

		return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
	}

	[Authorize(Roles = "Admin")]
	[HttpPut("{id:guid}")]
	public async Task<ActionResult> Update(Guid id, [FromBody] UpdateAirportCommand command)
	{
		command.Id = id;
		var result = await Mediator.Send(command);
		return HandleResult(result);
	}
}

