
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Cars.Commands;
using OnlineTravel.Application.Features.Cars.DTOs;
using OnlineTravel.Application.Features.Cars.Queries;
using OnlineTravel.Domain.Exceptions;
using OnlineTravelBookingTeamB.Models;

namespace OnlineTravelBookingTeamB.Controllers
{
    public class CarMvcController : Controller
    {
        private readonly IMediator _mediator;

        public CarMvcController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: Cars
        public async Task<IActionResult> Index(GetAllCarsSummaryQuery query)
        {
            var result = await _mediator.Send(query);
            if (result.IsSuccess)
                return View(result.Value);

            TempData["Error"] = result.Error.Description;
            return View(new PaginatedResult<CarSummaryDto>(1, 10, 0, new List<CarSummaryDto>()));
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var query = new GetCarDetailsByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            if (result.IsSuccess)
                return View(result.Value);

            TempData["Error"] = result.Error.Description;
            return RedirectToAction(nameof(Index));
        }

        // GET: Cars/Create
        public IActionResult Create()
        {
            return View(new CreateCarRequest());
        }

        // POST: Cars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCarRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var command = new CreateCarCommand { Data = request };
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                TempData["Success"] = "Car created successfully.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, result.Error.Description);
            return View(request);
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var query = new GetCarByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Error.Description;
                return RedirectToAction(nameof(Index));
            }

            // تحويل CarDto إلى UpdateCarRequest (استخدم Mapster إن أمكن)
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
            return View(updateRequest);
        }

        // POST: Cars/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UpdateCarRequest request)
        {
            if (id != request.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(request);

            var command = new UpdateCarCommand { Data = request };
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                TempData["Success"] = "Car updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, result.Error.Description);
            return View(request);
        }

        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var query = new GetCarByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Error.Description;
                return RedirectToAction(nameof(Index));
            }
            return View(result.Value);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var command = new DeleteCarCommand { Id = id };
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                TempData["Success"] = "Car deleted successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = result.Error.Description;
            return RedirectToAction(nameof(Delete), new { id });
        }
    }
}
