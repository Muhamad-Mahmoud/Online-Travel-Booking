using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Admin.Dashboard;

namespace OnlineTravel.Mvc.Controllers;

public class AdminController : BaseController
{
	public async Task<IActionResult> Index()
	{
		var result = await Mediator.Send(new GetAdminDashboardStatsQuery());

		if (result.IsSuccess)
		{
			return View("~/Views/Admin/Index.cshtml", result.Value);
		}

		return View("~/Views/Admin/Index.cshtml", new AdminDashboardResponse()); // Or handle error
	}
}
