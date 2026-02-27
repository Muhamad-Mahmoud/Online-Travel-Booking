using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Cars.GetAllCarsSummary;
using OnlineTravel.Application.Features.CarBrands.GetCarBrands;

namespace OnlineTravel.Mvc.Controllers;

public class CarsController : BaseController
{
	public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string? searchTerm = null, Guid? brandId = null)
	{
		var result = await Mediator.Send(new GetAllCarsSummaryQuery(pageIndex, pageSize, brandId, null, null, searchTerm));

		// Fetch brands and categories for filtering (assuming existing queries)
		var brandsResult = await Mediator.Send(new GetCarBrandsQuery());
		// var categoriesResult = await Mediator.Send(new GetCategoriesQuery(CategoryType.Car));

		ViewBag.SearchTerm = searchTerm;
		ViewBag.BrandId = brandId;
		ViewBag.Brands = brandsResult.IsSuccess ? brandsResult.Value : new List<OnlineTravel.Application.Features.CarBrands.Shared.DTOs.CarBrandDto>();
		// ViewBag.Categories = categoriesResult.IsSuccess ? categoriesResult.Value : new List<OnlineTravel.Application.Features.Categories.Shared.DTOs.CategoryDto>();

		return View("~/Views/Admin/Cars/Cars/Index.cshtml", result.Value);
	}
}
