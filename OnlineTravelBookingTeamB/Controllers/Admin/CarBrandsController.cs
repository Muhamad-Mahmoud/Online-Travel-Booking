using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.CarBrands.CreateCarBrand;
using OnlineTravel.Application.Features.CarBrands.UpdateCarBrand;
using OnlineTravel.Application.Features.CarBrands.DeleteCarBrand;
using OnlineTravel.Application.Features.CarBrands.GetCarBrandById;
using OnlineTravel.Application.Features.CarBrands.GetCarBrandsPaginated;
using OnlineTravel.Application.Features.CarBrands.Shared.DTOs;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineTravelBookingTeamB.Controllers.Admin
{
    [Route("Admin/CarBrands")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CarBrandsController : Controller
    {
        private readonly IMediator _mediator;

        public CarBrandsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: CarBrand
        [HttpGet]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 5, string? searchTerm = null)
        {
            var query = new GetCarBrandsPaginatedQuery(pageIndex, pageSize, searchTerm);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Error.Description;
                return View("~/Views/Admin/Cars/Brands/Index.cshtml", new PaginatedResult<CarBrandDto>(pageIndex, pageSize, 0, new List<CarBrandDto>()));
            }

            return View("~/Views/Admin/Cars/Brands/Index.cshtml", result.Value);
        }

        // GET: CarBrand/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var query = new GetCarBrandByIdQuery(id);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Error.Description;
                return RedirectToAction(nameof(Index));
            }

            return View("~/Views/Admin/Cars/Brands/Details.cshtml", result.Value);
        }

        // GET: CarBrand/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View("~/Views/Admin/Cars/Brands/Create.cshtml", new CreateCarBrandRequest());
        }

        // POST: CarBrand/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCarBrandRequest request)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Admin/Cars/Brands/Create.cshtml", request);

            var command = new CreateCarBrandCommand(request);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Error.Description);
                return View("~/Views/Admin/Cars/Brands/Create.cshtml", request);
            }

            TempData["Success"] = "Car brand created successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: CarBrand/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var query = new GetCarBrandByIdQuery(id);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Error.Description;
                return RedirectToAction(nameof(Index));
            }

            var updateRequest = result.Value.Adapt<UpdateCarBrandRequest>();
            return View("~/Views/Admin/Cars/Brands/Edit.cshtml", updateRequest);
        }

        // POST: CarBrand/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UpdateCarBrandRequest request)
        {
            if (id != request.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View("~/Views/Admin/Cars/Brands/Edit.cshtml", request);

            var command = new UpdateCarBrandCommand(id, request);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Error.Description);
                return View("~/Views/Admin/Cars/Brands/Edit.cshtml", request);
            }

            TempData["Success"] = "Car brand updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: CarBrand/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var query = new GetCarBrandByIdQuery(id);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Error.Description;
                return RedirectToAction(nameof(Index));
            }

            return View("~/Views/Admin/Cars/Brands/Delete.cshtml", result.Value);
        }

        // POST: CarBrand/Delete/5
        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var command = new DeleteCarBrandCommand(id);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Error.Description;
            }
            else
            {
                TempData["Success"] = "Car brand deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
