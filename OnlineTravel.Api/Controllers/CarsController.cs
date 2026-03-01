using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Cars.CreateCar;
using OnlineTravel.Application.Features.Cars.DeleteCar;
using OnlineTravel.Application.Features.Cars.GetAllCars;
using OnlineTravel.Application.Features.Cars.GetAllCarsSummary;
using OnlineTravel.Application.Features.Cars.GetCarById;
using OnlineTravel.Application.Features.Cars.GetCarByIdWithDetails;
using OnlineTravel.Application.Features.Cars.UpdateCar;

namespace OnlineTravel.Api.Controllers;

[Route("api/v1/cars")]
public class CarsController : BaseApiController
{
	/// <summary>
	/// List available cars with pagination and filters.
	/// </summary>
	[HttpGet]
	public async Task<ActionResult> GetAll([FromQuery] GetAllCarsQuery query)
	{
		var result = await Mediator.Send(query);
		return HandleResult(result);
	}

	/// <summary>
	/// Get a summary of available cars.
	/// </summary>
	[HttpGet("summary")]
	public async Task<ActionResult> GetAllSummary([FromQuery] GetAllCarsSummaryQuery query)
	{
		var result = await Mediator.Send(query);
		return HandleResult(result);
	}

	/// <summary>
	/// Get comprehensive details for a specific car.
	/// </summary>
	[HttpGet("{id}/details")]
	public async Task<ActionResult> GetByIdWithDetails(Guid id)
	{
		var query = new GetCarDetailsByIdQuery(id);
		var result = await Mediator.Send(query);
		return HandleResult(result);
	}

	/// <summary>
	/// Get basic car details by ID (Admin only).
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpGet("{id:guid}")]
	public async Task<ActionResult> GetById(Guid id)
	{
		var result = await Mediator.Send(new GetCarByIdQuery(id));
		return HandleResult(result);
	}

	/// <summary>
	/// Create a new car entry (Admin only).
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpPost]
	public async Task<ActionResult> Create([FromBody] CreateCarRequest request)
	{
		var result = await Mediator.Send(new CreateCarCommand(request));
		return HandleResult(result);
	}

	/// <summary>
	/// Update an existing car's information (Admin only).
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpPut("{id:guid}")]
	public async Task<ActionResult> Update(Guid id, [FromBody] UpdateCarRequest request)
	{
		request.Id = id;
		var result = await Mediator.Send(new UpdateCarCommand(request));
		return HandleResult(result);
	}

	/// <summary>
	/// Delete a car by ID (Admin only).
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpDelete("{id:guid}")]
	public async Task<ActionResult> Delete(Guid id)
	{
		var result = await Mediator.Send(new DeleteCarCommand(id));
		return HandleResult(result);
	}
}

