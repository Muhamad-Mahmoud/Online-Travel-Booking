using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Mvc.Helpers;
using OnlineTravel.Domain.Enums;
using OnlineTravel.Application.Features.Categories.GetCategoriesByType;
using OnlineTravel.Mvc.Models;
using OnlineTravel.Application.Features.Tours.GetAllTours;
using OnlineTravel.Application.Features.Tours.GetTourById;
using OnlineTravel.Application.Features.Tours.Manage.UpdateTour;
using OnlineTravel.Application.Features.Tours.Manage.AddActivity;
using OnlineTravel.Application.Features.Tours.Manage.AddPriceTier;
using OnlineTravel.Application.Features.Tours.DeleteTour;

namespace OnlineTravel.Mvc.Controllers;

public class ToursController : BaseController
{
	public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 5, string? search = null)
	{
		var result = await Mediator.Send(new GetAllToursQuery(pageIndex, pageSize, search, null, null, null, null, null, null, null, null, null));
		ViewBag.SearchTerm = search;
		return View(result);
	}

	public async Task<IActionResult> Create()
	{
		var categories = await Mediator.Send(new GetCategoriesByTypeQuery(CategoryType.Tour));
		ViewBag.Categories = categories.IsSuccess ? new Microsoft.AspNetCore.Mvc.Rendering.SelectList(categories.Value, "Id", "Title") : null;
		return View(new TourCreateViewModel());
	}

	[HttpPost]
	public async Task<IActionResult> Create(TourCreateViewModel model)
	{
		if (!ModelState.IsValid)
		{
			var categories = await Mediator.Send(new GetCategoriesByTypeQuery(CategoryType.Tour));
			ViewBag.Categories = categories.IsSuccess ? new Microsoft.AspNetCore.Mvc.Rendering.SelectList(categories.Value, "Id", "Title") : null;
			return View(model);
		}

		if (model.ImageFile != null)
		{
			model.MainImage = await FileUploadHelper.UploadFileAsync(model.ImageFile, "tours");
		}

		var result = await Mediator.Send(model);

		if (result.IsSuccess)
		{
			TempData["Success"] = "Tour Created Successfully!";
			return RedirectToAction(nameof(Index));
		}

		ModelState.AddModelError(string.Empty, result.Error.Description ?? "An error occurred");
		var categoriesOnError = await Mediator.Send(new GetCategoriesByTypeQuery(CategoryType.Tour));
		ViewBag.Categories = categoriesOnError.IsSuccess ? new Microsoft.AspNetCore.Mvc.Rendering.SelectList(categoriesOnError.Value, "Id", "Title") : null;
		return View(model);
	}

	public async Task<IActionResult> Manage(Guid id)
	{
		var result = await Mediator.Send(new GetTourByIdQuery(id));
		if (result.IsSuccess)
		{
			var tour = result.Value;
			var viewModel = new ToursManageViewModel
			{
				Tour = new TourDto
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
					Location = tour.Location != null ? new LocationDto { City = tour.Location.City ?? string.Empty, Country = tour.Location.Country ?? string.Empty, Street = tour.Location.Street ?? string.Empty } : null,
					Activities = [.. tour.Activities?.Select(a => new ActivityDto { Title = a.Title, Description = a.Description, ImageUrl = a.ImageUrl }) ?? []],
					PriceTiers = [.. tour.PriceTiers?.Select(p => new PriceTierDto { Id = p.Id, Name = p.Name, Price = p.Price.Amount }) ?? []],
					Images = [.. tour.Images?.Select(i => new ImageDto { Url = i.Url, AltText = i.AltText ?? "" }) ?? []]
				},
				EditForm = new TourEditFormDto
				{
					Id = tour.Id,
					TourId = tour.Id,
					Title = tour.Title,
					Description = tour.Description,
					DurationDays = tour.DurationDays,
					DurationNights = tour.DurationNights
				},
				ActivityForm = new ActivityFormDto { TourId = tour.Id },
				PriceTierForm = new PriceTierFormDto { TourId = tour.Id }
			};

			return View(viewModel);
		}
		return NotFound();
	}

	public async Task<IActionResult> Edit(Guid id)
	{
		var result = await Mediator.Send(new GetTourByIdQuery(id));
		if (!result.IsSuccess) return NotFound();

		var tour = result.Value;
		var viewModel = new TourEditFormDto
		{
			Id = tour.Id,
			TourId = tour.Id,
			Title = tour.Title,
			Description = tour.Description,
			DurationDays = tour.DurationDays,
			DurationNights = tour.DurationNights,
			CurrentImageUrl = tour.MainImageUrl
		};

		return View(viewModel);
	}

	[HttpPost]
	public async Task<IActionResult> Edit(TourEditFormDto model)
	{
		if (!ModelState.IsValid)
		{
			return View(model);
		}

		if (model.ImageFile != null)
		{
			model.MainImageUrl = await FileUploadHelper.UploadFileAsync(model.ImageFile, "tours");
		}

		var command = new UpdateTourCommand
		{
			TourId = model.Id,
			Title = model.Title ?? string.Empty,
			Description = model.Description ?? string.Empty,
			DurationDays = model.DurationDays,
			DurationNights = model.DurationNights,
			MainImageUrl = model.MainImageUrl ?? model.CurrentImageUrl
		};

		var result = await Mediator.Send(command);
		if (result.IsSuccess)
		{
			TempData["Success"] = "Tour Updated Successfully!";
			return RedirectToAction(nameof(Manage), new { id = model.Id });
		}

		ModelState.AddModelError(string.Empty, result.Error.Description);
		return View(model);
	}

	[HttpPost]
	public async Task<IActionResult> Update(ToursManageViewModel model)
	{
		var editForm = model.EditForm;
		if (editForm.ImageFile != null)
		{
			editForm.MainImageUrl = await FileUploadHelper.UploadFileAsync(editForm.ImageFile, "tours");
		}

		var command = new UpdateTourCommand
		{
			TourId = editForm.TourId,
			Title = editForm.Title ?? string.Empty,
			Description = editForm.Description ?? string.Empty,
			DurationDays = editForm.DurationDays,
			DurationNights = editForm.DurationNights,
			MainImageUrl = editForm.MainImageUrl ?? editForm.CurrentImageUrl
		};

		var result = await Mediator.Send(command);
		if (result.IsSuccess)
		{
			TempData["Success"] = "Tour Updated Successfully!";
		}
		else
		{
			TempData["Error"] = result.Error.Description;
		}

		return RedirectToAction(nameof(Manage), new { id = editForm.TourId });
	}

	[HttpPost]
	public async Task<IActionResult> AddActivity(ToursManageViewModel model)
	{
		var activityForm = model.ActivityForm;
		if (activityForm.ImageFile != null)
		{
			activityForm.Image = await FileUploadHelper.UploadFileAsync(activityForm.ImageFile, "activities");
		}

		var command = new AddTourActivityCommand
		{
			TourId = activityForm.TourId,
			Title = activityForm.Title,
			Description = activityForm.Description,
			ImageUrl = activityForm.Image ?? string.Empty
		};

		var result = await Mediator.Send(command);
		if (result.IsSuccess)
		{
			TempData["Success"] = "Activity Added Successfully!";
		}
		else
		{
			TempData["Error"] = result.Error.Description;
		}

		return RedirectToAction(nameof(Manage), new { id = activityForm.TourId });
	}

	[HttpPost]
	public async Task<IActionResult> AddPriceTier(ToursManageViewModel model)
	{
		var priceTierForm = model.PriceTierForm;
		var command = new AddTourPriceTierCommand
		{
			TourId = priceTierForm.TourId,
			Name = priceTierForm.Name,
			Amount = priceTierForm.Amount
		};

		var result = await Mediator.Send(command);
		if (result.IsSuccess)
		{
			TempData["Success"] = "Price Tier Added Successfully!";
		}
		else
		{
			TempData["Error"] = result.Error.Description;
		}

		return RedirectToAction(nameof(Manage), new { id = priceTierForm.TourId });
	}

	public async Task<IActionResult> Delete(Guid id)
	{
		var result = await Mediator.Send(new GetTourByIdQuery(id));
		if (!result.IsSuccess) return NotFound();

		var tour = result.Value;
		var viewModel = new TourDto
		{
			Id = tour.Id,
			Title = tour.Title,
			Description = tour.Description,
			Category = tour.Category
		};

		return View(viewModel);
	}

	[HttpPost, ActionName("Delete")]
	public async Task<IActionResult> DeleteConfirmed(Guid id)
	{
		var result = await Mediator.Send(new DeleteTourCommand(id));
		if (result.IsSuccess)
		{
			TempData["Success"] = "Tour Deleted Successfully!";
		}
		else
		{
			TempData["Error"] = "Failed to Delete Tour.";
		}
		return RedirectToAction(nameof(Index));
	}
}

