using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Tours.DeleteTour.Commands;
using OnlineTravel.Application.Features.Tours.GetAllTours.Queries;
using OnlineTravel.Application.Features.Tours.GetTourById.Queries;
using OnlineTravel.Application.Features.Tours.CreateTour.Commands;
using OnlineTravel.Application.Features.Categories.GetCategoriesByType;
using OnlineTravel.Domain.Enums;

namespace OnlineTravel.Mvc.Controllers;

public class ToursController : BaseController
{
	public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string? search = null)
	{
		var result = await Mediator.Send(new GetAllToursQuery(pageIndex, pageSize, search, null, null, null, null, null, null, null, null, null));
		ViewBag.SearchTerm = search;
		return View("~/Views/Admin/Tours/Tours/Index.cshtml", result);
	}

	public async Task<IActionResult> Create()
	{
		var categories = await Mediator.Send(new GetCategoriesByTypeQuery(CategoryType.Tour));
		ViewBag.Categories = categories.IsSuccess ? new Microsoft.AspNetCore.Mvc.Rendering.SelectList(categories.Value, "Id", "Title") : null;
		return View("~/Views/Admin/Tours/Tours/Create.cshtml", new CreateTourCommand());
	}

	[HttpPost]
	public async Task<IActionResult> Create(CreateTourCommand command)
	{
		if (!ModelState.IsValid)
		{
			var categories = await Mediator.Send(new GetCategoriesByTypeQuery(CategoryType.Tour));
			ViewBag.Categories = categories.IsSuccess ? new Microsoft.AspNetCore.Mvc.Rendering.SelectList(categories.Value, "Id", "Title") : null;
			return View("~/Views/Admin/Tours/Tours/Create.cshtml", command);
		}

		var result = await Mediator.Send(command);

		if (result.IsSuccess)
		{
			TempData["Success"] = "Tour created successfully";
			return RedirectToAction(nameof(Index));
		}

		ModelState.AddModelError(string.Empty, result.Error.Description ?? "An error occurred");
		var categoriesOnError = await Mediator.Send(new GetCategoriesByTypeQuery(CategoryType.Tour));
		ViewBag.Categories = categoriesOnError.IsSuccess ? new Microsoft.AspNetCore.Mvc.Rendering.SelectList(categoriesOnError.Value, "Id", "Title") : null;
		return View("~/Views/Admin/Tours/Tours/Create.cshtml", command);
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
