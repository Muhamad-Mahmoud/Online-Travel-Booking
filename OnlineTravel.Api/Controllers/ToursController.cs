using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Tours.CreateTour;
using OnlineTravel.Application.Features.Tours.DeleteTour;
using OnlineTravel.Application.Features.Tours.GetAllTours;
using OnlineTravel.Application.Features.Tours.GetTourById;
using OnlineTravel.Application.Features.Tours.Manage.AddActivity;
using OnlineTravel.Application.Features.Tours.Manage.AddImage;
using OnlineTravel.Application.Features.Tours.Manage.AddPriceTier;
using OnlineTravel.Application.Features.Tours.Manage.UpdateCoordinates;
using OnlineTravel.Application.Features.Tours.Manage.UpdateTour;

namespace OnlineTravel.Api.Controllers;

[Route("api/v1/tours")]
public class ToursController : BaseApiController
{
	/// <summary>
	/// List all tours with custom filters and pagination.
	/// </summary>
	[HttpGet]
	public async Task<ActionResult> GetAll([FromQuery] GetAllToursQuery query)
	{
		var result = await Mediator.Send(query);
		return Ok(result);
	}

	/// <summary>
	/// Get details of a specific tour by ID.
	/// </summary>
	[HttpGet("{id}")]
	public async Task<ActionResult> GetById(Guid id)
	{
		var result = await Mediator.Send(new GetTourByIdQuery(id));
		return HandleResult(result);
	}

	/// <summary>
	/// Create a new tour package (Admin only).
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpPost]
	public async Task<ActionResult> Create([FromBody] CreateTourCommand command)
	{
		var result = await Mediator.Send(command);
		if (!result.IsSuccess)
			return HandleResult(result);

		return CreatedAtAction(nameof(GetById), new { id = result.Value }, new { id = result.Value });
	}

	/// <summary>
	/// Update an existing tour's details (Admin only).
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpPut("{id:guid}")]
	public async Task<ActionResult> Update(Guid id, [FromBody] UpdateTourCommand command)
	{
		command.TourId = id;
		var result = await Mediator.Send(command);
		return HandleResult(result);
	}

	/// <summary>
	/// Delete a tour by ID (Admin only).
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpDelete("{id:guid}")]
	public async Task<ActionResult> Delete(Guid id)
	{
		var result = await Mediator.Send(new DeleteTourCommand(id));
		if (!result.IsSuccess)
			return HandleResult(result);
		return NoContent();
	}

	/// <summary>
	/// Add an activity to a tour (Admin only).
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpPost("{id:guid}/activities")]
	public async Task<ActionResult> AddActivity(Guid id, [FromBody] AddTourActivityCommand command)
	{
		command.TourId = id;
		var result = await Mediator.Send(command);
		if (!result.IsSuccess)
			return HandleResult(result);

		return CreatedAtAction(nameof(GetById), new { id }, new { id = result.Value });
	}

	/// <summary>
	/// Add a gallery image to a tour (Admin only).
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpPost("{id:guid}/images")]
	public async Task<ActionResult> AddImage(Guid id, [FromBody] AddTourImageCommand command)
	{
		command.TourId = id;
		var result = await Mediator.Send(command);
		if (!result.IsSuccess)
			return HandleResult(result);

		return CreatedAtAction(nameof(GetById), new { id }, new { id = result.Value });
	}

	/// <summary>
	/// Add a pricing tier to a tour (Admin only).
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpPost("{id:guid}/price-tiers")]
	public async Task<ActionResult> AddPriceTier(Guid id, [FromBody] AddTourPriceTierCommand command)
	{
		command.TourId = id;
		var result = await Mediator.Send(command);
		if (!result.IsSuccess)
			return HandleResult(result);

		return CreatedAtAction(nameof(GetById), new { id }, new { id = result.Value });
	}

	/// <summary>
	/// Update the geographic coordinates for a tour (Admin only).
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpPut("{id:guid}/coordinates")]
	public async Task<ActionResult> UpdateCoordinates(Guid id, [FromBody] UpdateTourCoordinatesCommand command)
	{
		command.TourId = id;
		var result = await Mediator.Send(command);
		return HandleResult(result);
	}
}

