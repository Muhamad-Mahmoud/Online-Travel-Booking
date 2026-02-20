using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.CarPricingTiers.Commands;
using OnlineTravel.Application.Features.CarPricingTiers.DTOs;
using OnlineTravel.Application.Features.CarPricingTiers.Queries;

namespace OnlineTravelBookingTeamB.Controllers
{
    public class CarPricingTiersMvcController : Controller
    {
        private readonly IMediator _mediator;

        public CarPricingTiersMvcController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: CarPricingTiers?carId=xxx
        public async Task<IActionResult> Index(Guid carId)
        {
            if (carId == Guid.Empty)
                return BadRequest("Car ID is required");

            var query = new GetAllCarPricingTiersQuery { CarId = carId };
            var result = await _mediator.Send(query);
            if (result.IsSuccess)
            {
                ViewBag.CarId = carId; // لاستخدامه في الروابط
                return View(result.Value);
            }

            TempData["Error"] = result.Error.Description;
            return RedirectToAction("Details", "Cars", new { id = carId });
        }

        // GET: CarPricingTiers/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var query = new GetCarPricingTierByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            if (result.IsSuccess)
                return View(result.Value);

            TempData["Error"] = result.Error.Description;
            return RedirectToAction(nameof(Index), new { carId = result.Value?.CarId });
        }

        // GET: CarPricingTiers/Create?carId=xxx
        public IActionResult Create(Guid carId)
        {
            if (carId == Guid.Empty)
                return BadRequest("Car ID is required");

            var request = new CreateCarPricingTierRequest
            {
                CarId = carId,
                PricePerHour = new MoneyDto { Amount = 0, Currency = "USD" }
            };
            return View(request);
        }

        // POST: CarPricingTiers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCarPricingTierRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var command = new CreateCarPricingTierCommand { Data = request };
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                TempData["Success"] = "Pricing tier created successfully.";
                return RedirectToAction(nameof(Index), new { carId = request.CarId });
            }

            ModelState.AddModelError(string.Empty, result.Error.Description);
            return View(request);
        }

        // GET: CarPricingTiers/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var query = new GetCarPricingTierByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Error.Description;
                return RedirectToAction(nameof(Index), new { carId = result.Value?.CarId });
            }

            // تحويل CarPricingTierDto إلى UpdateCarPricingTierRequest
            var updateRequest = new UpdateCarPricingTierRequest
            {
                Id = result.Value.Id,
                CarId = result.Value.CarId,
                FromHours = result.Value.FromHours,
                ToHours = result.Value.ToHours,
                PricePerHour = result.Value.PricePerHour
            };
            return View(updateRequest);
        }

        // POST: CarPricingTiers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UpdateCarPricingTierRequest request)
        {
            if (id != request.Id)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return View(request);

            var command = new UpdateCarPricingTierCommand { Data = request };
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                TempData["Success"] = "Pricing tier updated successfully.";
                return RedirectToAction(nameof(Index), new { carId = request.CarId });
            }

            ModelState.AddModelError(string.Empty, result.Error.Description);
            return View(request);
        }

        // GET: CarPricingTiers/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var query = new GetCarPricingTierByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Error.Description;
                return RedirectToAction("Index", "Cars");
            }
            return View(result.Value);
        }

        // POST: CarPricingTiers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            // نحتاج لمعرفة CarId قبل الحذف لإعادة التوجيه
            var entity = await _mediator.Send(new GetCarPricingTierByIdQuery { Id = id });
            if (!entity.IsSuccess)
            {
                TempData["Error"] = entity.Error.Description;
                return RedirectToAction("Index", "Cars");
            }

            var command = new DeleteCarPricingTierCommand { Id = id };
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                TempData["Success"] = "Pricing tier deleted successfully.";
                return RedirectToAction(nameof(Index), new { carId = entity.Value.CarId });
            }

            TempData["Error"] = result.Error.Description;
            return RedirectToAction(nameof(Delete), new { id });
        }
    }
}
