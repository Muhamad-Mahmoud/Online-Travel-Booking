using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Hotels.Admin.GetHotels;

namespace OnlineTravel.Mvc.Controllers;

public class HotelsController : BaseController
{
	public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string? search = null)
	{
		var result = await Mediator.Send(new GetHotelsQuery(pageIndex, pageSize, search));
		ViewBag.SearchTerm = search;
		return View("~/Views/Admin/Hotels/Index.cshtml", result.Value);
	}
}
