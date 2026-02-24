
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Cars.Commands;
using OnlineTravel.Application.Features.Cars.DTOs;
using OnlineTravel.Application.Features.Cars.Queries;
using OnlineTravel.Domain.Exceptions;
using OnlineTravelBookingTeamB.Models;

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
            if (result.IsSuccess)
                return View("~/Views/Admin/Cars/Cars/Index.cshtml", result.Value);

            TempData["Error"] = result.Error.Description;
            return View("~/Views/Admin/Cars/Cars/Index.cshtml", new PaginatedResult<CarSummaryDto>(1, 10, 0, new List<CarSummaryDto>()));
        }

        // GET: Cars/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var query = new GetCarDetailsByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            if (result.IsSuccess)
                return View("~/Views/Admin/Cars/Cars/Details.cshtml", result.Value);

            TempData["Error"] = result.Error.Description;
            return RedirectToAction(nameof(Index));
        }

        // GET: Cars/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View("~/Views/Admin/Cars/Cars/Create.cshtml", new CreateCarRequest());
        }

        // POST: Cars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCarRequest request)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Admin/Cars/Cars/Create.cshtml", request);

            var command = new CreateCarCommand { Data = request };
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                TempData["Success"] = "Car created successfully.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, result.Error.Description);
            return View("~/Views/Admin/Cars/Cars/Create.cshtml", request);
        }

        // GET: Cars/Edit/5
        [HttpGet("Edit/{id}")]
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
                return View("~/Views/Admin/Cars/Cars/Edit.cshtml", request);

            var command = new UpdateCarCommand { Data = request };
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                TempData["Success"] = "Car updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, result.Error.Description);
            return View("~/Views/Admin/Cars/Cars/Edit.cshtml", request);
        }

        // GET: Cars/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var query = new GetCarByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Error.Description;
                return RedirectToAction(nameof(Index));
            }
            return View("~/Views/Admin/Cars/Cars/Delete.cshtml", result.Value);
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
