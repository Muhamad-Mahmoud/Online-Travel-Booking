using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Tours.DeleteTour.Commands;
using OnlineTravel.Application.Features.Tours.GetAllTours.Queries;
using OnlineTravel.Application.Features.Tours.GetTourById.Queries;

namespace OnlineTravel.Mvc.Controllers;

public class ToursController : BaseController
{
	public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string? search = null)
	{
		var result = await Mediator.Send(new GetAllToursQuery(pageIndex, pageSize, search, null, null, null, null, null, null, null, null, null));
		ViewBag.SearchTerm = search;
		return View("~/Views/Admin/Tours/Tours/Index.cshtml", result);
	}

	public async Task<IActionResult> Manage(Guid id)
	{
		var result = await Mediator.Send(new GetTourByIdQuery(id));
		if (result.IsSuccess)
		{
			return View("~/Views/Admin/Tours/Tours/Manage.cshtml", result.Value);
		}
		return NotFound();
	}

	[HttpPost]
	public async Task<IActionResult> Delete(Guid id)
	{
		var result = await Mediator.Send(new DeleteTourCommand(id));
		if (result.IsSuccess)
		{
			TempData["Success"] = "Tour deleted successfully.";
		}
		else
		{
			TempData["Error"] = "Failed to delete tour.";
		}
		return RedirectToAction(nameof(Index));
	}
}
