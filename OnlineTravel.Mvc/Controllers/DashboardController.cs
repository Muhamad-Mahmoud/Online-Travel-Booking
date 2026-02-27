using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Admin.Dashboard;

namespace OnlineTravel.Mvc.Controllers;

public class DashboardController : BaseController
{
	public async Task<IActionResult> Index()
	{
		var result = await Mediator.Send(new GetAdminDashboardStatsQuery());

		if (result.IsSuccess)
		{
			return View(result.Value);
		}

		return View(new AdminDashboardResponse()); // Or handle error
	}
}
