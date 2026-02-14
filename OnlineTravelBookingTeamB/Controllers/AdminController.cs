using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Infrastructure.Persistence.Context;

namespace OnlineTravelBookingTeamB.Controllers
{
    // [Authorize(Roles = "Admin")] // Uncomment when roles are ready
    public class AdminController : Controller
    {
        private readonly OnlineTravelDbContext _context; // Keeping for queries for now
        private readonly MediatR.IMediator _mediator;

        public AdminController(OnlineTravelDbContext context, MediatR.IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SeedData()
        {
            return View();
        }

        public IActionResult Monitor()
        {
            return View();
        }

        public async Task<IActionResult> Tours()
        {
            var query = new OnlineTravel.Application.Features.Tours.GetAllTours.Queries.GetAllToursQuery(1, 50, null, null, null, null, null, null, null, null, null, null);
            var result = await _mediator.Send(query);
            return View(result.Data);
        }

        [HttpGet]
        public IActionResult CreateTour()
        {
            ViewBag.Categories = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Categories.Where(c => c.Type == OnlineTravel.Domain.Enums.CategoryType.Tour), "Id", "Title");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTour(Models.TourViewModel model)
        {
            if (ModelState.IsValid)
            {
                try 
                {
                    var command = new OnlineTravel.Application.Features.Tours.CreateTour.Commands.CreateTourCommand
                    {
                        Title = model.Title,
                        Description = model.Description,
                        CategoryId = model.CategoryId,
                        DurationDays = model.DurationDays,
                        DurationNights = model.DurationNights,
                        Recommended = model.Recommended,
                        BestTimeToVisit = model.BestTimeToVisit,
                        Street = model.Street,
                        City = model.City,
                        State = model.State,
                        Country = model.Country,
                        PostalCode = model.PostalCode,
                        MainImageUrl = model.MainImageUrl,
                        StandardPrice = model.StandardPrice,
                        Currency = model.Currency
                    };

                    await _mediator.Send(command);
                    return RedirectToAction(nameof(Tours));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error creating tour: " + ex.Message);
                }
            }
            ViewBag.Categories = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Categories.Where(c => c.Type == OnlineTravel.Domain.Enums.CategoryType.Tour), "Id", "Title");
            return View(model);
        }

        public IActionResult Hotels()
        {
            return View();
        }

        public IActionResult Cars()
        {
            return View();
        }

        public IActionResult Flights()
        {
            return View();
            }
}

}
