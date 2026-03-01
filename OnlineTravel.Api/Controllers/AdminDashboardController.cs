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
	/// <summary>
	/// Get high-level administrative statistics for the dashboard.
	/// </summary>
	[HttpGet("dashboard")]
	public async Task<ActionResult> GetStats()
	{
		var result = await Mediator.Send(new GetAdminDashboardStatsQuery());
		return HandleResult(result);
	}

	/// <summary>
	/// Get categories filtered by type for administrative tasks.
	/// </summary>
	[HttpGet("categories/by-type")]
	public async Task<ActionResult> GetCategoriesByType([FromQuery] CategoryType type)
	{
		var result = await Mediator.Send(new AdminGetCategoriesByTypeQuery(type));
		return HandleResult(result);
	}
}

