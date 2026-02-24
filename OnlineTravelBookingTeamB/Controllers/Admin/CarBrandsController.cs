using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.CarBrands.Commands;
using OnlineTravel.Application.Features.CarBrands.DTOs;
using OnlineTravel.Application.Features.CarBrands.Queries;
using OnlineTravel.Domain.Exceptions;

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
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string searchTerm = null)
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
                TempData["Error"] = result.Error.Description; // تعديل
                return RedirectToAction(nameof(Index));
            }

            return View("~/Views/Admin/Cars/Brands/Details.cshtml", result.Value);
        }

        // GET: CarBrand/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View("~/Views/Admin/Cars/Brands/Create.cshtml", new CreateCarBrandDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCarBrandDto dto)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Admin/Cars/Brands/Create.cshtml", dto);

            var command = new CreateCarBrandCommand(dto);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Error.Description);
                return View("~/Views/Admin/Cars/Brands/Create.cshtml", dto);
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
                TempData["Error"] = result.Error.Description; // تعديل
                return RedirectToAction(nameof(Index));
            }

            // تحويل CarBrandDto إلى UpdateCarBrandDto (باستخدام Mapster)
            var updateDto = result.Value.Adapt<UpdateCarBrandDto>();
            return View("~/Views/Admin/Cars/Brands/Edit.cshtml", updateDto);
        }

        // POST: CarBrand/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UpdateCarBrandDto dto)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Admin/Cars/Brands/Edit.cshtml", dto);

            var command = new UpdateCarBrandCommand(id, dto);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Error.Description); // تعديل
                return View("~/Views/Admin/Cars/Brands/Edit.cshtml", dto);
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
                TempData["Error"] = result.Error.Description; // تعديل
                return RedirectToAction(nameof(Index));
            }

            return View("~/Views/Admin/Cars/Brands/Delete.cshtml", result.Value);
        }

        // POST: CarBrand/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var command = new DeleteCarBrandCommand(id);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Error.Description; // تعديل
            }
            else
            {
                TempData["Success"] = "Car brand deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

