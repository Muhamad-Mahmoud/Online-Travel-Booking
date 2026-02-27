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
		return View(result);
	}

	public async Task<IActionResult> Create()
	{
		var categories = await Mediator.Send(new GetCategoriesByTypeQuery(CategoryType.Tour));
		ViewBag.Categories = categories.IsSuccess ? new Microsoft.AspNetCore.Mvc.Rendering.SelectList(categories.Value, "Id", "Title") : null;
		return View(new CreateTourCommand());
	}

	[HttpPost]
	public async Task<IActionResult> Create(CreateTourCommand command)
	{
		if (!ModelState.IsValid)
		{
			var categories = await Mediator.Send(new GetCategoriesByTypeQuery(CategoryType.Tour));
			ViewBag.Categories = categories.IsSuccess ? new Microsoft.AspNetCore.Mvc.Rendering.SelectList(categories.Value, "Id", "Title") : null;
			return View(command);
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
		return View(command);
	}

	public async Task<IActionResult> Manage(Guid id)
	{
		var result = await Mediator.Send(new GetTourByIdQuery(id));
		if (result.IsSuccess)
		{
			var tour = result.Value;
			var viewModel = new OnlineTravel.Mvc.Models.ToursManageViewModel
			{
				Tour = new OnlineTravel.Mvc.Models.TourDto
				{
					Id = tour.Id,
					Title = tour.Title,
					Description = tour.Description,
					DurationDays = tour.DurationDays,
					DurationNights = tour.DurationNights,
					Category = tour.Category,
					MainImageUrl = tour.MainImageUrl,
					Rating = (decimal)tour.Rating,
					Recommended = tour.Recommended,
					Location = tour.Location != null ? new OnlineTravel.Mvc.Models.LocationDto { City = tour.Location.City, Country = tour.Location.Country, Street = tour.Location.Street } : null,
					Activities = tour.Activities.Select(a => new OnlineTravel.Mvc.Models.ActivityDto { Title = a.Title, Description = a.Description, ImageUrl = a.ImageUrl }).ToList(),
					PriceTiers = tour.PriceTiers.Select(p => new OnlineTravel.Mvc.Models.PriceTierDto { Id = p.Id, Name = p.Name, Price = p.Price.Amount }).ToList(),
					Images = tour.Images.Select(i => new OnlineTravel.Mvc.Models.ImageDto { Url = i.Url, AltText = i.AltText ?? "" }).ToList()
				},
				EditForm = new OnlineTravel.Mvc.Models.TourEditFormDto
				{
					Id = tour.Id,
					TourId = tour.Id,
					Title = tour.Title,
					Description = tour.Description,
					DurationDays = tour.DurationDays,
					DurationNights = tour.DurationNights
				},
				ActivityForm = new OnlineTravel.Mvc.Models.ActivityFormDto { TourId = tour.Id },
				PriceTierForm = new OnlineTravel.Mvc.Models.PriceTierFormDto { TourId = tour.Id }
			};

			return View(viewModel);
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
