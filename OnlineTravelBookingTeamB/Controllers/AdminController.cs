using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Infrastructure.Persistence.Context;

namespace OnlineTravelBookingTeamB.Controllers
{
    // [Authorize(Roles = "Admin")] // Uncomment when roles are ready
    public class AdminController : Controller
    {
        private readonly OnlineTravelDbContext _context; // Keeping for queries for now
        private readonly MediatR.IMediator _mediator;
        private readonly OnlineTravel.Application.Interfaces.Services.IFileService _fileService;

        public AdminController(OnlineTravelDbContext context, MediatR.IMediator mediator, OnlineTravel.Application.Interfaces.Services.IFileService fileService)
        {
            _context = context;
            _mediator = mediator;
            _fileService = fileService;
        }

        public IActionResult Index()
        {
            var model = new Models.DashboardViewModel
            {
                TotalTours = _context.Tours.Count(),
                TotalHotels = 12, // Mock data
                TotalCars = 45,   // Mock data
                TotalFlights = 8, // Mock data
                TotalRevenue = 12450.00m,
                ActiveBookings = 34,
                NewUsersToday = 5,
                RecentActivities = new List<Models.RecentActivityItem>
                {
                    new Models.RecentActivityItem { Title = "New Booking #1024", Description = "Flight to Paris confirmed", TimeAgo = "2 mins ago", Status = "success", Icon = "bi-check-circle" },
                    new Models.RecentActivityItem { Title = "System Alert", Description = "High server load detected", TimeAgo = "15 mins ago", Status = "warning", Icon = "bi-exclamation-triangle" },
                    new Models.RecentActivityItem { Title = "New User", Description = "John Doe registered", TimeAgo = "1 hour ago", Status = "info", Icon = "bi-person-plus" },
                    new Models.RecentActivityItem { Title = "Payment Failed", Description = "Booking #1023 payment declined", TimeAgo = "3 hours ago", Status = "danger", Icon = "bi-x-circle" }
                }
            };
            return View("Dashboard/Index", model);
        }

        public IActionResult SeedData()
        {
            return View("System/SeedData");
        }

        public IActionResult Monitor()
        {
            return View("Dashboard/Monitor");
        }

        public async Task<IActionResult> Tours()
        {
            var query = new OnlineTravel.Application.Features.Tours.GetAllTours.Queries.GetAllToursQuery(1, 50, null, null, null, null, null, null, null, null, null, null);
            var result = await _mediator.Send(query);
            return View("Tours/Index", result.Data);
        }

        [HttpGet]
        public IActionResult CreateTour()
        {
            ViewBag.Categories = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Categories.Where(c => c.Type == OnlineTravel.Domain.Enums.CategoryType.Tour), "Id", "Title");
            return View("Tours/Create");
        }

        [HttpPost]
        public async Task<IActionResult> CreateTour(Models.TourViewModel model)
        {
            if (ModelState.IsValid)
            {
                try 
                {
                    string mainImageUrl = string.Empty;
                    if (model.MainImage != null)
                    {
                        using (var stream = model.MainImage.OpenReadStream())
                        {
                            mainImageUrl = await _fileService.UploadFileAsync(stream, model.MainImage.FileName, "tours");
                        }
                    }

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
                        MainImageUrl = mainImageUrl,
                        StandardPrice = model.StandardPrice,
                        Currency = model.Currency
                    };

                    var tourId = await _mediator.Send(command);
                    return RedirectToAction(nameof(ManageTour), new { id = tourId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error creating tour: " + ex.Message);
                }
            }
            ViewBag.Categories = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Categories.Where(c => c.Type == OnlineTravel.Domain.Enums.CategoryType.Tour), "Id", "Title");
            return View("Tours/Create", model);
        }

        [HttpGet]
        public async Task<IActionResult> ManageTour(Guid id)
        {
            var query = new OnlineTravel.Application.Features.Tours.GetTourById.Queries.GetTourByIdQuery(id);
            var tour = await _mediator.Send(query);

            if (tour == null)
            {
                return NotFound();
            }

            var model = new OnlineTravelBookingTeamB.Models.ManageTourViewModel
            {
                Tour = tour,
                ActivityForm = new OnlineTravelBookingTeamB.Models.AddActivityViewModel { TourId = id },
                ImageForm = new OnlineTravelBookingTeamB.Models.AddTourImageViewModel { TourId = id },
                PriceTierForm = new OnlineTravel.Application.Features.Tours.Manage.Commands.AddPriceTier.AddTourPriceTierCommand { TourId = id }
            };

            return View("Tours/Manage", model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTour(Guid id)
        {
            try
            {
                var command = new OnlineTravel.Application.Features.Tours.DeleteTour.Commands.DeleteTourCommand(id);
                await _mediator.Send(command);
                // Optionally add a success message to TempData
            }
            catch (Exception ex)
            {
                // Handle error (e.g., log it, show error message)
                // TempData["Error"] = "Error deleting tour: " + ex.Message;
            }
            return RedirectToAction(nameof(Tours));
        }

        [HttpPost]
        public async Task<IActionResult> AddTourActivity([Bind(Prefix = "ActivityForm")] OnlineTravelBookingTeamB.Models.AddActivityViewModel model)
        {
            if (ModelState.IsValid)
            {
                string imageUrl = string.Empty;
                if (model.Image != null)
                {
                    using (var stream = model.Image.OpenReadStream())
                    {
                        imageUrl = await _fileService.UploadFileAsync(stream, model.Image.FileName, "tours/activities");
                    }
                }

                var command = new OnlineTravel.Application.Features.Tours.Manage.Commands.AddActivity.AddTourActivityCommand
                {
                    TourId = model.TourId,
                    Title = model.Title,
                    Description = model.Description,
                    ImageUrl = imageUrl
                };

                await _mediator.Send(command);
            }
            return RedirectToAction(nameof(ManageTour), new { id = model.TourId });
        }

        [HttpPost]
        public async Task<IActionResult> AddTourImage([Bind(Prefix = "ImageForm")] OnlineTravelBookingTeamB.Models.AddTourImageViewModel model)
        {
            if (ModelState.IsValid)
            {
                string imageUrl = string.Empty;
                if (model.Image != null)
                {
                    using (var stream = model.Image.OpenReadStream())
                    {
                        imageUrl = await _fileService.UploadFileAsync(stream, model.Image.FileName, "tours/gallery");
                    }
                }

                var command = new OnlineTravel.Application.Features.Tours.Manage.Commands.AddImage.AddTourImageCommand
                {
                    TourId = model.TourId,
                    Url = imageUrl,
                    AltText = model.AltText
                };

                await _mediator.Send(command);
            }
            return RedirectToAction(nameof(ManageTour), new { id = model.TourId });
        }

        [HttpPost]
        public async Task<IActionResult> AddTourPriceTier([Bind(Prefix = "PriceTierForm")] OnlineTravel.Application.Features.Tours.Manage.Commands.AddPriceTier.AddTourPriceTierCommand command)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(command);
            }
            return RedirectToAction(nameof(ManageTour), new { id = command.TourId });
        }

        public IActionResult Hotels()
        {
            return View("Hotels/Index");
        }

        public IActionResult Cars()
        {
            return View("Cars/Index");
        }

        public IActionResult Flights()
        {
            return View("Flights/Index");
        }
}

}
