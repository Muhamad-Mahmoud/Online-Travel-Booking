using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Domain.Enums;
using GetCategoriesByTypeQuery = OnlineTravel.Application.Features.Categories.GetCategoriesByType.GetCategoriesByTypeQuery;

namespace OnlineTravel.Api.Controllers;

[Route("api/v1/categories")]
public class CategoriesController : BaseApiController
{
	/// <summary>
	/// Get a list of categories filtered by their type (e.g., Flight, Hotel).
	/// </summary>
	[HttpGet("by-type")]
	public async Task<ActionResult> GetByType([FromQuery] CategoryType type)
	{
		var result = await Mediator.Send(new GetCategoriesByTypeQuery(type));
		return HandleResult(result);
	}
}

