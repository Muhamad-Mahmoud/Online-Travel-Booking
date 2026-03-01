using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Admin.Dashboard;
using OnlineTravel.Domain.Enums;
using AdminGetCategoriesByTypeQuery = OnlineTravel.Application.Features.Categories.GetCategoriesByType.GetCategoriesByTypeQuery;

namespace OnlineTravel.Api.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/v1/admin")]
public class AdminDashboardController : BaseApiController
{
	[HttpGet("dashboard")]
	public async Task<ActionResult> GetStats()
	{
		var result = await Mediator.Send(new GetAdminDashboardStatsQuery());
		return HandleResult(result);
	}

	[HttpGet("categories/by-type")]
	public async Task<ActionResult> GetCategoriesByType([FromQuery] CategoryType type)
	{
		var result = await Mediator.Send(new AdminGetCategoriesByTypeQuery(type));
		return HandleResult(result);
	}
}

