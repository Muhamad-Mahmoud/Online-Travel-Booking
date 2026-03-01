using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.CarBrands.CreateCarBrand;
using OnlineTravel.Application.Features.CarBrands.DeleteCarBrand;
using OnlineTravel.Application.Features.CarBrands.GetCarBrandById;
using OnlineTravel.Application.Features.CarBrands.GetCarBrandsPaginated;
using OnlineTravel.Application.Features.CarBrands.UpdateCarBrand;

namespace OnlineTravel.Api.Controllers;

[Route("api/v1/car-brands")]
[Authorize(Roles = "Admin")]
public class CarBrandsController : BaseApiController
{
	/// <summary>
	/// List car brands with pagination (Admin only).
	/// </summary>
	[HttpGet]
	public async Task<ActionResult> GetAll([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null)
	{
		var result = await Mediator.Send(new GetCarBrandsPaginatedQuery(pageIndex, pageSize, searchTerm));
		return HandleResult(result);
	}

	/// <summary>
	/// Get a car brand's details by ID (Admin only).
	/// </summary>
	[HttpGet("{id:guid}")]
	public async Task<ActionResult> GetById(Guid id)
	{
		var result = await Mediator.Send(new GetCarBrandByIdQuery(id));
		return HandleResult(result);
	}

	/// <summary>
	/// Create a new car brand (Admin only).
	/// </summary>
	[HttpPost]
	public async Task<ActionResult> Create([FromBody] CreateCarBrandRequest request)
	{
		var result = await Mediator.Send(new CreateCarBrandCommand(request));
		return HandleResult(result);
	}

	/// <summary>
	/// Update an existing car brand (Admin only).
	/// </summary>
	[HttpPut("{id:guid}")]
	public async Task<ActionResult> Update(Guid id, [FromBody] UpdateCarBrandRequest request)
	{
		request.Id = id;
		var result = await Mediator.Send(new UpdateCarBrandCommand(id, request));
		return HandleResult(result);
	}

	/// <summary>
	/// Delete a car brand (Admin only).
	/// </summary>
	[HttpDelete("{id:guid}")]
	public async Task<ActionResult> Delete(Guid id)
	{
		var result = await Mediator.Send(new DeleteCarBrandCommand(id));
		return HandleResult(result);
	}
}

