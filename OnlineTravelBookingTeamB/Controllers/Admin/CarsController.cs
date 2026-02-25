using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Cars.CreateCar;
using OnlineTravel.Application.Features.Cars.UpdateCar;
using OnlineTravel.Application.Features.Cars.DeleteCar;
using OnlineTravel.Application.Features.Cars.GetCarById;
using OnlineTravel.Application.Features.Cars.GetCarByIdWithDetails;
using OnlineTravel.Application.Features.Cars.GetAllCarsSummary;
using OnlineTravel.Application.Features.CarBrands.GetCarBrandsPaginated;
using OnlineTravel.Application.Features.CarBrands.Shared.DTOs;
using OnlineTravel.Application.Features.Categories.GetCategoriesByType;
using OnlineTravel.Application.Features.Categories.Shared.DTOs;
using OnlineTravel.Domain.Enums;
using OnlineTravel.Domain.Exceptions;
using OnlineTravelBookingTeamB.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineTravelBookingTeamB.Controllers.Admin
{
    [Route("Admin/Cars")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CarsController : Controller
    {
        private readonly IMediator _mediator;

        public CarsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: Cars
        [HttpGet]
        public async Task<IActionResult> Index(GetAllCarsSummaryQuery query)
        {
            var result = await _mediator.Send(query);
            
            // Fetch brands and categories for filtering
            var brandsResult = await _mediator.Send(new GetCarBrandsPaginatedQuery(1, 100));
            ViewBag.Brands = brandsResult.IsSuccess ? brandsResult.Value.Items : new List<CarBrandDto>();

            var categoriesResult = await _mediator.Send(new GetCategoriesByTypeQuery(CategoryType.Car));
            ViewBag.Categories = categoriesResult.IsSuccess ? categoriesResult.Value : new List<CategoryDto>();

            ViewBag.SearchTerm = query.SearchTerm;
            ViewBag.BrandId = query.BrandId;

            if (result.IsSuccess)
                return View("~/Views/Admin/Cars/Cars/Index.cshtml", result.Value);

            TempData["Error"] = result.Error.Description;
            return View("~/Views/Admin/Cars/Cars/Index.cshtml", new PaginatedResult<CarSummaryDto>(1, 5, 0, new List<CarSummaryDto>()));
        }

        // GET: Cars/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var query = new GetCarDetailsByIdQuery(id);
            var result = await _mediator.Send(query);
            if (result.IsSuccess)
                return View("~/Views/Admin/Cars/Cars/Details.cshtml", result.Value);

            TempData["Error"] = result.Error.Description;
            return RedirectToAction(nameof(Index));
        }

        // GET: Cars/Create
        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            var brandsResult = await _mediator.Send(new GetCarBrandsPaginatedQuery(1, 100));
            ViewBag.Brands = brandsResult.IsSuccess ? brandsResult.Value.Items : new List<CarBrandDto>();
            
            var categoriesResult = await _mediator.Send(new GetCategoriesByTypeQuery(CategoryType.Car));
            ViewBag.Categories = categoriesResult.IsSuccess ? categoriesResult.Value : new List<CategoryDto>();

            return View("~/Views/Admin/Cars/Cars/Create.cshtml", new CreateCarRequest());
        }

        // POST: Cars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCarRequest request)
        {
            if (!ModelState.IsValid)
            {
                var brandsResult = await _mediator.Send(new GetCarBrandsPaginatedQuery(1, 100));
                ViewBag.Brands = brandsResult.IsSuccess ? brandsResult.Value.Items : new List<CarBrandDto>();
                var categoriesResult = await _mediator.Send(new GetCategoriesByTypeQuery(CategoryType.Car));
                ViewBag.Categories = categoriesResult.IsSuccess ? categoriesResult.Value : new List<CategoryDto>();
                return View("~/Views/Admin/Cars/Cars/Create.cshtml", request);
            }

            var command = new CreateCarCommand(request);
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                TempData["Success"] = "Car created successfully.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, result.Error.Description);
            var bRes = await _mediator.Send(new GetCarBrandsPaginatedQuery(1, 100));
            ViewBag.Brands = bRes.IsSuccess ? bRes.Value.Items : new List<CarBrandDto>();
            var cRes = await _mediator.Send(new GetCategoriesByTypeQuery(CategoryType.Car));
            ViewBag.Categories = cRes.IsSuccess ? cRes.Value : new List<CategoryDto>();
            return View("~/Views/Admin/Cars/Cars/Create.cshtml", request);
        }

        // GET: Cars/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var query = new GetCarByIdQuery(id);
            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Error.Description;
                return RedirectToAction(nameof(Index));
            }

            var brandsResult = await _mediator.Send(new GetCarBrandsPaginatedQuery(1, 100));
            ViewBag.Brands = brandsResult.IsSuccess ? brandsResult.Value.Items : new List<CarBrandDto>();

            var categoriesResult = await _mediator.Send(new GetCategoriesByTypeQuery(CategoryType.Car));
            ViewBag.Categories = categoriesResult.IsSuccess ? categoriesResult.Value : new List<CategoryDto>();

            var updateRequest = new UpdateCarRequest
            {
                Id = result.Value.Id,
                BrandId = result.Value.BrandId,
                Make = result.Value.Make,
                Model = result.Value.Model,
                CarType = result.Value.CarType,
                SeatsCount = result.Value.SeatsCount,
                FuelType = result.Value.FuelType,
                Transmission = result.Value.Transmission,
                Features = result.Value.Features,
                AvailableDates = result.Value.AvailableDates,
                CancellationPolicy = result.Value.CancellationPolicy,
                CategoryId = result.Value.CategoryId,
                Location = result.Value.Location,
                Images = result.Value.Images
            };
            return View("~/Views/Admin/Cars/Cars/Edit.cshtml", updateRequest);
        }

        // POST: Cars/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UpdateCarRequest request)
        {
            if (id != request.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                var brandsResult = await _mediator.Send(new GetCarBrandsPaginatedQuery(1, 100));
                ViewBag.Brands = brandsResult.IsSuccess ? brandsResult.Value.Items : new List<CarBrandDto>();
                var categoriesResult = await _mediator.Send(new GetCategoriesByTypeQuery(CategoryType.Car));
                ViewBag.Categories = categoriesResult.IsSuccess ? categoriesResult.Value : new List<CategoryDto>();
                return View("~/Views/Admin/Cars/Cars/Edit.cshtml", request);
            }

            var command = new UpdateCarCommand(request);
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                TempData["Success"] = "Car updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, result.Error.Description);
            var bRes = await _mediator.Send(new GetCarBrandsPaginatedQuery(1, 100));
            ViewBag.Brands = bRes.IsSuccess ? bRes.Value.Items : new List<CarBrandDto>();
            var cRes = await _mediator.Send(new GetCategoriesByTypeQuery(CategoryType.Car));
            ViewBag.Categories = cRes.IsSuccess ? cRes.Value : new List<CategoryDto>();
            return View("~/Views/Admin/Cars/Cars/Edit.cshtml", request);
        }

        // GET: Cars/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteCarCommand(id);
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                TempData["Success"] = "Car deleted successfully.";
            }
            else
            {
                TempData["Error"] = result.Error.Description;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
