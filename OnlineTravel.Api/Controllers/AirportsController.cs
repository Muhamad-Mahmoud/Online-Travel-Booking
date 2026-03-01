using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Flights.Airport.GetAirportById;
using OnlineTravel.Application.Features.Flights.Airport.GetAllAirports;
using OnlineTravel.Application.Features.Flights.Airport.UpdateAirport;
using OnlineTravel.Application.Features.Flights.Airport.DeleteAirport;
using OnlineTravel.Application.Features.Flights.CreateAirport;

namespace OnlineTravel.Api.Controllers;

[Route("api/v1/flights/airports")]
public class AirportsController : BaseApiController
{
	/// <summary>
	/// Get a paginated list of all airports (Admin only).
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpGet]
	public async Task<ActionResult> GetAll([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 100)
	{
		if (pageIndex <= 0 || pageSize <= 0)
			return BadRequest("pageIndex and pageSize must be greater than 0.");

		var result = await Mediator.Send(new GetAllAirportsQuery { PageIndex = pageIndex, PageSize = pageSize });
		return HandleResult(result);
	}

	/// <summary>
	/// Get details for a specific airport by ID (Admin only).
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpGet("{id:guid}")]
	public async Task<ActionResult> GetById(Guid id)
	{
		var result = await Mediator.Send(new GetAirportByIdQuery(id));
		return HandleResult(result);
	}

	/// <summary>
	/// Create a new airport entry (Admin only).
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpPost]
	public async Task<ActionResult> Create([FromBody] CreateAirportCommand command)
	{
		var result = await Mediator.Send(command);
		if (!result.IsSuccess)
			return HandleResult(result);

		return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
	}

	/// <summary>
	/// Update information for an existing airport (Admin only).
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpPut("{id:guid}")]
	public async Task<ActionResult> Update(Guid id, [FromBody] UpdateAirportCommand command)
	{
		command.Id = id;
		var result = await Mediator.Send(command);
		return HandleResult(result);
	}

	/// <summary>
	/// Delete an airport by ID (Admin only).
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpDelete("{id:guid}")]
	public async Task<ActionResult> Delete(Guid id)
	{
		var result = await Mediator.Send(new DeleteAirportCommand(id));
		return HandleResult(result);
	}
}

