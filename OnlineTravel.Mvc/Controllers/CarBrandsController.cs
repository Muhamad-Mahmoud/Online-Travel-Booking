using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Mvc.Helpers;
using OnlineTravel.Mvc.Models;
using OnlineTravel.Application.Features.CarBrands.CreateCarBrand;
using OnlineTravel.Application.Features.CarBrands.DeleteCarBrand;
using OnlineTravel.Application.Features.CarBrands.GetCarBrandById;
using OnlineTravel.Application.Features.CarBrands.GetCarBrandsPaginated;
using OnlineTravel.Application.Features.CarBrands.UpdateCarBrand;

namespace OnlineTravel.Mvc.Controllers;

public class CarBrandsController : BaseController
{
    public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 5, string? searchTerm = null)
    {
        var result = await Mediator.Send(new GetCarBrandsPaginatedQuery(pageIndex, pageSize, searchTerm));
        return View("~/Views/Cars/Brands/Index.cshtml", result.Value);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var result = await Mediator.Send(new GetCarBrandByIdQuery(id));
        if (!result.IsSuccess) return NotFound();
        return View("~/Views/Cars/Brands/Details.cshtml", result.Value);
    }

    public IActionResult Create()
    {
        return View("~/Views/Cars/Brands/Create.cshtml", new BrandCreateViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(BrandCreateViewModel model)
    {
        if (!ModelState.IsValid) return View("~/Views/Cars/Brands/Create.cshtml", model);

        if (model.LogoFile != null)
        {
            model.Logo = await FileUploadHelper.UploadFileAsync(model.LogoFile, "brands");
        }

        var result = await Mediator.Send(new CreateCarBrandCommand(model));
        if (result.IsSuccess)
        {
            TempData["Success"] = "Brand Created Successfully!";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty, result.Error.Description);
        return View("~/Views/Cars/Brands/Create.cshtml", model);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var result = await Mediator.Send(new GetCarBrandByIdQuery(id));
        if (!result.IsSuccess) return NotFound();

        var brand = result.Value;
        var model = new BrandEditViewModel
        {
            Id = brand.Id,
            Name = brand.Name,
            Logo = brand.Logo,
            IsActive = brand.IsActive
        };

        return View("~/Views/Cars/Brands/Edit.cshtml", model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(BrandEditViewModel model)
    {
        if (!ModelState.IsValid) return View("~/Views/Cars/Brands/Edit.cshtml", model);

        if (model.LogoFile != null)
        {
            model.Logo = await FileUploadHelper.UploadFileAsync(model.LogoFile, "brands");
        }

        var result = await Mediator.Send(new UpdateCarBrandCommand(model.Id, model));
        if (result.IsSuccess)
        {
            TempData["Success"] = "Brand Updated Successfully!";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty, result.Error.Description);
        return View("~/Views/Cars/Brands/Edit.cshtml", model);
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await Mediator.Send(new GetCarBrandByIdQuery(id));
        if (!result.IsSuccess) return NotFound();
        return View("~/Views/Cars/Brands/Delete.cshtml", result.Value);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var result = await Mediator.Send(new DeleteCarBrandCommand(id));
        if (result.IsSuccess)
        {
            TempData["Success"] = "Brand Deleted Successfully!";
            return RedirectToAction(nameof(Index));
        }

        TempData["Error"] = result.Error.Description;
        return RedirectToAction(nameof(Index));
    }
}
