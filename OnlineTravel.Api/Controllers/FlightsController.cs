using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Flights.Flights.CreateFlight;
using OnlineTravel.Application.Features.Flights.Flights.GetFlightById;
using OnlineTravel.Application.Features.Flights.Flights.GetFlights;
using OnlineTravel.Application.Features.Flights.Flights.Manage;
using OnlineTravel.Application.Features.Flights.Flights.SearchFlights.Queries;
using OnlineTravel.Application.Features.Flights.Flights.UpdateFlight;

namespace OnlineTravel.Api.Controllers;

[Route("api/v1/flights")]
public class FlightsController : BaseApiController
{
	/// <summary>
	/// Search for available flights based on origin, destination, and dates.
	/// </summary>
	[HttpGet]
	public async Task<ActionResult> Search([FromQuery] SearchFlightsQuery query)
	{
		var result = await Mediator.Send(query);
		return Ok(result);
	}

	/// <summary>
	/// Create a new flight schedule (Admin only).
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpPost]
	public async Task<ActionResult> Create([FromBody] CreateFlightCommand command)
	{
		var result = await Mediator.Send(command);
		if (!result.IsSuccess)
			return HandleResult(result);

		return Ok(new { id = result.Value });
	}

	/// <summary>
	/// List all flights with pagination and search/status filters (Admin only).
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpGet("admin")]
	public async Task<ActionResult> GetAll([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null, [FromQuery] string? status = null)
	{
		var result = await Mediator.Send(new GetFlightsQuery(pageIndex, pageSize, search, status));
		return HandleResult(result);
	}

	/// <summary>
	/// Get detailed information for a specific flight by ID (Admin only).
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpGet("admin/{id:guid}")]
	public async Task<ActionResult> GetById(Guid id)
	{
		var result = await Mediator.Send(new GetFlightByIdQuery(id));
		return HandleResult(result);
	}

	/// <summary>
	/// Update an existing flight's details (Admin only).
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpPut("{id:guid}")]
	public async Task<ActionResult> Update(Guid id, [FromBody] UpdateFlightCommand command)
	{
		command.Id = id;
		var result = await Mediator.Send(command);
		return HandleResult(result);
	}

	/// <summary>
	/// Add seats to a specific flight (Admin only).
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpPost("{id:guid}/seats")]
	public async Task<ActionResult> AddSeat(Guid id, [FromBody] AddSeatCommand command)
	{
		var commandWithId = command with { FlightId = id };
		var result = await Mediator.Send(commandWithId);
		return HandleResult(result);
	}

	/// <summary>
	/// Remove a seat from a specific flight (Admin only).
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpDelete("{id:guid}/seats/{seatId:guid}")]
	public async Task<ActionResult> DeleteSeat(Guid id, Guid seatId)
	{
		var result = await Mediator.Send(new DeleteSeatCommand(seatId));
		return HandleResult(result);
	}

	/// <summary>
	/// Add a fare option to a specific flight (Admin only).
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpPost("{id:guid}/fares")]
	public async Task<ActionResult> AddFare(Guid id, [FromBody] AddFareCommand command)
	{
		var commandWithId = command with { FlightId = id };
		var result = await Mediator.Send(commandWithId);
		return HandleResult(result);
	}

	/// <summary>
	/// Remove a fare option from a specific flight (Admin only).
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpDelete("{id:guid}/fares/{fareId:guid}")]
	public async Task<ActionResult> DeleteFare(Guid id, Guid fareId)
	{
		var result = await Mediator.Send(new DeleteFareCommand(fareId));
		return HandleResult(result);
	}
}

