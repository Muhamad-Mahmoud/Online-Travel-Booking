using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Mvc.Helpers;
using OnlineTravel.Mvc.Models;
using OnlineTravel.Application.Features.Cars.Shared;
using OnlineTravel.Application.Features.Cars.GetAllCarsSummary;
using OnlineTravel.Application.Features.CarBrands.GetCarBrands;
using OnlineTravel.Application.Features.Cars.CreateCar;
using OnlineTravel.Application.Features.Cars.DeleteCar;
using OnlineTravel.Application.Features.Cars.GetCarById;
using OnlineTravel.Application.Features.Cars.GetCarByIdWithDetails;
using OnlineTravel.Application.Features.Categories.GetCategoriesByType;
using OnlineTravel.Domain.Enums;
using OnlineTravel.Application.Features.Cars.UpdateCar;

namespace OnlineTravel.Mvc.Controllers;

public class CarsController : BaseController
{
	public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 5, string? searchTerm = null, Guid? brandId = null)
	{
		var result = await Mediator.Send(new GetAllCarsSummaryQuery(pageIndex, pageSize, brandId, null, null, searchTerm));

		var brandsResult = await Mediator.Send(new GetCarBrandsQuery());
		var categoriesResult = await Mediator.Send(new GetCategoriesByTypeQuery(CategoryType.Car));

		ViewBag.SearchTerm = searchTerm;
		ViewBag.BrandId = brandId;
		ViewBag.Brands = brandsResult.IsSuccess ? brandsResult.Value : [];
		ViewBag.Categories = categoriesResult.IsSuccess ? categoriesResult.Value : [];

		return View("~/Views/Cars/Cars/Index.cshtml", result.Value);
	}

	public async Task<IActionResult> Details(Guid id)
	{
		var result = await Mediator.Send(new GetCarDetailsByIdQuery(id));
		if (!result.IsSuccess) return NotFound();

		return View("~/Views/Cars/Cars/Details.cshtml", result.Value);
	}

	public async Task<IActionResult> Create()
	{
		var brandsResult = await Mediator.Send(new GetCarBrandsQuery());
		var categoriesResult = await Mediator.Send(new GetCategoriesByTypeQuery(CategoryType.Car));

		ViewBag.Brands = brandsResult.IsSuccess ? brandsResult.Value : [];
		ViewBag.Categories = categoriesResult.IsSuccess ? categoriesResult.Value : [];

		return View("~/Views/Cars/Cars/Create.cshtml", new CarCreateViewModel());
	}

	[HttpPost]
	public async Task<IActionResult> Create(CarCreateViewModel command)
	{
		if (!ModelState.IsValid)
		{
			var brandsResult = await Mediator.Send(new GetCarBrandsQuery());
			var categoriesResult = await Mediator.Send(new GetCategoriesByTypeQuery(CategoryType.Car));
			ViewBag.Brands = brandsResult.IsSuccess ? brandsResult.Value : [];
			ViewBag.Categories = categoriesResult.IsSuccess ? categoriesResult.Value : [];
			return View("~/Views/Cars/Cars/Create.cshtml", command);
		}
// turbo
		if (command.ImageFiles != null && command.ImageFiles.Count > 0)
		{
			foreach (var file in command.ImageFiles)
			{
				var path = await FileUploadHelper.UploadFileAsync(file, "cars");
				if (path != null)
				{
					command.Images.Add(new ImageUrlResponse { Url = path });
				}
			}
		}

		var result = await Mediator.Send(new CreateCarCommand(command));

		if (result.IsSuccess)
		{
			TempData["Success"] = "Car Created Successfully!";
			return RedirectToAction(nameof(Index));
		}

		ModelState.AddModelError(string.Empty, result.Error.Description);
		var brandsResultOnError = await Mediator.Send(new GetCarBrandsQuery());
		var categoriesResultOnError = await Mediator.Send(new GetCategoriesByTypeQuery(CategoryType.Car));
		ViewBag.Brands = brandsResultOnError.IsSuccess ? brandsResultOnError.Value : [];
		ViewBag.Categories = categoriesResultOnError.IsSuccess ? categoriesResultOnError.Value : [];
		return View("~/Views/Cars/Cars/Create.cshtml", command);
	}

	public async Task<IActionResult> Edit(Guid id)
	{
		var result = await Mediator.Send(new GetCarByIdQuery(id));
		if (!result.IsSuccess) return NotFound();

		var car = result.Value;
		if (car == null) return NotFound();
		
		var updateRequest = new CarEditViewModel
		{
			Id = car.Id,
			BrandId = car.BrandId,
			Make = car.Make,
			Model = car.Model,
			CarType = car.CarType,
			SeatsCount = car.SeatsCount,
			FuelType = car.FuelType,
			Transmission = car.Transmission,
			Features = car.Features,
			AvailableDates = car.AvailableDates,
			CancellationPolicy = car.CancellationPolicy,
			CategoryId = car.CategoryId,
			Location = car.Location,
			Images = car.Images
		};

		var brandsResult = await Mediator.Send(new GetCarBrandsQuery());
		var categoriesResult = await Mediator.Send(new GetCategoriesByTypeQuery(CategoryType.Car));
		
		ViewBag.Brands = brandsResult.IsSuccess ? brandsResult.Value : [];
		ViewBag.Categories = categoriesResult.IsSuccess ? categoriesResult.Value : [];

		return View("~/Views/Cars/Cars/Edit.cshtml", updateRequest);
	}

	[HttpPost]
	public async Task<IActionResult> Edit(CarEditViewModel command)
	{
		if (command.ImageFiles != null && command.ImageFiles.Count > 0)
		{
			foreach (var file in command.ImageFiles)
			{
				var path = await FileUploadHelper.UploadFileAsync(file, "cars");
				if (path != null)
				{
					command.Images.Add(new ImageUrlResponse { Url = path });
				}
			}
		}

		var result = await Mediator.Send(new UpdateCarCommand(command));
		if (result.IsSuccess)
		{
			TempData["Success"] = "Car Updated Successfully!";
			return RedirectToAction("Index");
		}

		var brandsResult = await Mediator.Send(new GetCarBrandsQuery());
		var categoriesResult = await Mediator.Send(new GetCategoriesByTypeQuery(CategoryType.Car));
		ViewBag.Brands = brandsResult.IsSuccess ? brandsResult.Value : [];
		ViewBag.Categories = categoriesResult.IsSuccess ? categoriesResult.Value : [];
		
		return View("~/Views/Cars/Cars/Edit.cshtml", command);
	}

	public async Task<IActionResult> Delete(Guid id)
	{
		var result = await Mediator.Send(new GetCarByIdQuery(id));
		if (!result.IsSuccess) return NotFound();

		return View("~/Views/Cars/Cars/Delete.cshtml", result.Value);
	}

	[HttpPost, ActionName("Delete")]
	public async Task<IActionResult> DeleteConfirmed(Guid id)
	{
		var result = await Mediator.Send(new DeleteCarCommand(id));
		if (result.IsSuccess)
		{
			TempData["Success"] = "Car Deleted Successfully!";
			return RedirectToAction(nameof(Index));
		}

		TempData["Error"] = result.Error.Description;
		return RedirectToAction(nameof(Index));
	}
}

