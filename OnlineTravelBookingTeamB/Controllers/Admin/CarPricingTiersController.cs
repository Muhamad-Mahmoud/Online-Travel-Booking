using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.CarPricingTiers.Common;
using OnlineTravel.Application.Features.CarPricingTiers.Create;
using OnlineTravel.Application.Features.CarPricingTiers.Delete;
using OnlineTravel.Application.Features.CarPricingTiers.GetAll;
using OnlineTravel.Application.Features.CarPricingTiers.GetById;
using OnlineTravel.Application.Features.CarPricingTiers.Update;
using CreatePricingTier = OnlineTravel.Application.Features.CarPricingTiers.Create;
using UpdatePricingTier = OnlineTravel.Application.Features.CarPricingTiers.Update;

namespace OnlineTravelBookingTeamB.Controllers.Admin
{
    [Route("Admin/CarPricingTiers")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CarPricingTiersController : Controller
    {
        private readonly IMediator _mediator;

        public CarPricingTiersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: CarPricingTiers?carId=xxx
        [HttpGet]
        public async Task<IActionResult> Index(Guid carId)
        {
            if (carId == Guid.Empty)
                return BadRequest("Car ID is required");

            var query = new GetAllCarPricingTiersQuery(carId);
            var result = await _mediator.Send(query);
            if (result.IsSuccess)
            {
                ViewBag.CarId = carId; // لاستخدامه في الروابط
                return View("~/Views/Admin/Cars/PricingTiers/Index.cshtml", result.Value);
            }

            TempData["Error"] = result.Error.Description;
            return RedirectToAction("Details", "Cars", new { id = carId });
        }

        // GET: CarPricingTiers/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var query = new GetCarPricingTierByIdQuery(id);
            var result = await _mediator.Send(query);
            if (result.IsSuccess)
                return View("~/Views/Admin/Cars/PricingTiers/Details.cshtml", result.Value);

            TempData["Error"] = result.Error.Description;
            return RedirectToAction(nameof(Index), new { carId = result.Value?.CarId });
        }

        // GET: CarPricingTiers/Create?carId=xxx
        [HttpGet("Create")]
        public IActionResult Create(Guid carId)
        {
            if (carId == Guid.Empty)
                return BadRequest("Car ID is required");

            var formModel = new CreateCarPricingTierFormModel
            {
                CarId = carId,
                PricePerHour = new CreatePricingTier.MoneyFormModel { Amount = 0, Currency = "USD" }
            };
            return View("~/Views/Admin/Cars/PricingTiers/Create.cshtml", formModel);
        }

        // POST: CarPricingTiers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCarPricingTierFormModel formModel)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Admin/Cars/PricingTiers/Create.cshtml", formModel);

            var command = new CreateCarPricingTierCommand(
                formModel.CarId,
                formModel.FromHours,
                formModel.ToHours,
                new MoneyCommand(formModel.PricePerHour.Amount, formModel.PricePerHour.Currency));

            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                TempData["Success"] = "Pricing tier created successfully.";
                return RedirectToAction(nameof(Index), new { carId = formModel.CarId });
            }

            ModelState.AddModelError(string.Empty, result.Error.Description);
            return View("~/Views/Admin/Cars/PricingTiers/Create.cshtml", formModel);
        }

        // GET: CarPricingTiers/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var query = new GetCarPricingTierByIdQuery(id);
            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Error.Description;
                return RedirectToAction(nameof(Index), new { carId = result.Value?.CarId });
            }

            // تحويل Response إلى UpdateCarPricingTierFormModel
            var updateFormModel = new UpdateCarPricingTierFormModel
            {
                Id = result.Value.Id,
                CarId = result.Value.CarId,
                FromHours = result.Value.FromHours,
                ToHours = result.Value.ToHours,
                PricePerHour = new UpdatePricingTier.MoneyFormModel
                {
                    Amount = result.Value.PricePerHour.Amount,
                    Currency = result.Value.PricePerHour.Currency
                }
            };
            return View("~/Views/Admin/Cars/PricingTiers/Edit.cshtml", updateFormModel);
        }

        // POST: CarPricingTiers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UpdateCarPricingTierFormModel formModel)
        {
            if (id != formModel.Id)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return View("~/Views/Admin/Cars/PricingTiers/Edit.cshtml", formModel);

            var command = new UpdateCarPricingTierCommand(
                formModel.Id,
                formModel.CarId,
                formModel.FromHours,
                formModel.ToHours,
                new MoneyCommand(formModel.PricePerHour.Amount, formModel.PricePerHour.Currency));

            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                TempData["Success"] = "Pricing tier updated successfully.";
                return RedirectToAction(nameof(Index), new { carId = formModel.CarId });
            }

            ModelState.AddModelError(string.Empty, result.Error.Description);
            return View("~/Views/Admin/Cars/PricingTiers/Edit.cshtml", formModel);
        }

        // GET: CarPricingTiers/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var query = new GetCarPricingTierByIdQuery(id);
            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Error.Description;
                return RedirectToAction("Index", "Cars");
            }
            return View("~/Views/Admin/Cars/PricingTiers/Delete.cshtml", result.Value);
        }

        // POST: CarPricingTiers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            // نحتاج لمعرفة CarId قبل الحذف لإعادة التوجيه
            var entity = await _mediator.Send(new GetCarPricingTierByIdQuery(id));
            if (!entity.IsSuccess)
            {
                TempData["Error"] = entity.Error.Description;
                return RedirectToAction("Index", "Cars");
            }

            var command = new DeleteCarPricingTierCommand(id);
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
