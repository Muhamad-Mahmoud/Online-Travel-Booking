using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.CarBrands.Commands;
using OnlineTravel.Application.Features.CarBrands.DTOs;
using OnlineTravel.Application.Features.CarBrands.Queries;
using OnlineTravel.Domain.Exceptions;

namespace OnlineTravelBookingTeamB.Controllers
{
    public class CarBrandMvcController : Controller
    {
        private readonly IMediator _mediator;

        public CarBrandMvcController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: CarBrand
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string searchTerm = null)
        {
            var query = new GetCarBrandsPaginatedQuery(pageIndex, pageSize, searchTerm);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Error.Description;
                return View(new PaginatedResult<CarBrandDto>(pageIndex, pageSize, 0, new List<CarBrandDto>()));
            }

            return View(result.Value);
        }

        // GET: CarBrand/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var query = new GetCarBrandByIdQuery(id);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Error.Description; // تعديل
                return RedirectToAction(nameof(Index));
            }

            return View(result.Value);
        }

        // GET: CarBrand/Create
        public IActionResult Create()
        {
            return View(new CreateCarBrandDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCarBrandDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var command = new CreateCarBrandCommand(dto);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Error.Description);
                return View(dto);
            }

            TempData["Success"] = "Car brand created successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: CarBrand/Edit/5
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
            return View(updateDto);
        }

        // POST: CarBrand/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UpdateCarBrandDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var command = new UpdateCarBrandCommand(id, dto);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Error.Description); // تعديل
                return View(dto);
            }

            TempData["Success"] = "Car brand updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: CarBrand/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var query = new GetCarBrandByIdQuery(id);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Error.Description; // تعديل
                return RedirectToAction(nameof(Index));
            }

            return View(result.Value);
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

