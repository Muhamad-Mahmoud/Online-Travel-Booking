using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineTravel.Application.Features.Bookings.DTOs;
using OnlineTravel.Application.Features.Admin.Export;
using OnlineTravel.Application.Features.Bookings.GetAllBookings;
using OnlineTravel.Application.Features.Bookings.GetBookingById;
using OnlineTravel.Application.Features.Bookings.GetUserBookings;
using OnlineTravel.Domain.Enums;
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

        public async Task<IActionResult> Index()
        {
            var query = new OnlineTravel.Application.Features.Admin.Dashboard.GetAdminDashboardStatsQuery();
            var result = await _mediator.Send(query);
            return View(result.Value);
        }


        public IActionResult SeedData()
        {
            return View("System/SeedData");
        }



        public async Task<IActionResult> Tours(int pageIndex = 1, int pageSize = 20)
        {
            var query = new OnlineTravel.Application.Features.Tours.GetAllTours.Queries.GetAllToursQuery(pageIndex, pageSize, null, null, null, null, null, null, null, null, null, null);
            var result = await _mediator.Send(query);
            return View("Tours/Index", result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> CreateTour()
        {
            var categories = await _context.Categories
                .Where(c => c.Type == CategoryType.Tour)
                .AsNoTracking()
                .ToListAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Title");
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
            var categories = await _context.Categories
                .Where(c => c.Type == CategoryType.Tour)
                .AsNoTracking()
                .ToListAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Title");
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
                EditForm = new OnlineTravelBookingTeamB.Models.EditTourViewModel
                {
                    TourId = id,
                    Title = tour.Title,
                    Description = tour.Description,
                    CategoryId = tour.CategoryId,
                    DurationDays = tour.DurationDays,
                    DurationNights = tour.DurationNights,
                    BestTimeToVisit = tour.BestTimeToVisit,
                    Recommended = tour.Recommended,
                    Street = tour.Location?.Street,
                    City = tour.Location?.City,
                    State = tour.Location?.State,
                    Country = tour.Location?.Country,
                    PostalCode = tour.Location?.PostalCode
                },
                ActivityForm = new OnlineTravelBookingTeamB.Models.AddActivityViewModel { TourId = id },
                ImageForm = new OnlineTravelBookingTeamB.Models.AddTourImageViewModel { TourId = id },
                PriceTierForm = new OnlineTravel.Application.Features.Tours.Manage.Commands.AddPriceTier.AddTourPriceTierCommand { TourId = id },
                LocationForm = new Models.UpdateCoordinatesViewModel 
                { 
                    TourId = id, 
                    Latitude = tour.Location?.Coordinates?.Y, 
                    Longitude = tour.Location?.Coordinates?.X 
                }
            };

            var categories = await _context.Categories
                .Where(c => c.Type == CategoryType.Tour)
                .AsNoTracking()
                .ToListAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Title", tour.CategoryId);
            return View("Tours/Manage", model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTour(Models.EditTourViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string? mainImageUrl = null;
                    if (model.MainImage != null)
                    {
                        using (var stream = model.MainImage.OpenReadStream())
                        {
                            mainImageUrl = await _fileService.UploadFileAsync(stream, model.MainImage.FileName, "tours");
                        }
                    }

                    var command = new OnlineTravel.Application.Features.Tours.Manage.Commands.UpdateTour.UpdateTourCommand
                    {
                        TourId = model.TourId,
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
                        MainImageUrl = mainImageUrl
                    };

                    await _mediator.Send(command);
                    TempData["Success"] = "Tour updated successfully!";
                    return RedirectToAction(nameof(ManageTour), new { id = model.TourId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error updating tour: " + ex.Message);
                }
            }
            return RedirectToAction(nameof(ManageTour), new { id = model.TourId });
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

        [HttpPost]
        public async Task<IActionResult> UpdateTourLocation(Models.UpdateCoordinatesViewModel model)
        {
            if (ModelState.IsValid)
            {
                var command = new OnlineTravel.Application.Features.Tours.Manage.Commands.UpdateCoordinates.UpdateTourCoordinatesCommand
                {
                    TourId = model.TourId,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude
                };
                await _mediator.Send(command);
            }
            return RedirectToAction(nameof(ManageTour), new { id = model.TourId });
        }

        public IActionResult Hotels()
        {
            return View("Hotels/Index");
        }

   
        public IActionResult Flights()
        {
            return View("Flights/Index");
        }


        public async Task<IActionResult> Bookings(int pageIndex = 1, int pageSize = 5, string? searchTerm = null, string? status = null)
        {
            var bookingsQuery = new GetAllBookingsQuery(pageIndex, pageSize, searchTerm, status);
            var statsQuery = new OnlineTravel.Application.Features.Bookings.GetBookingStats.GetBookingStatsQuery();

            // Run both queries in parallel — they are completely independent
            var bookingsTask = _mediator.Send(bookingsQuery);
            var statsTask    = _mediator.Send(statsQuery);
            await Task.WhenAll(bookingsTask, statsTask);

            var bookingsResult = await bookingsTask;
            var statsResult    = await statsTask;

            var viewModel = new Models.AdminBookingsViewModel
            {
                Bookings = bookingsResult.IsSuccess ? bookingsResult.Value : new OnlineTravel.Application.Common.PagedResult<AdminBookingResponse>(new List<AdminBookingResponse>(), 0, pageIndex, pageSize),
                Stats = statsResult.IsSuccess ? statsResult.Value : new BookingStatsDto()
            };

            ViewBag.SearchTerm = searchTerm;
            ViewBag.Status = status;
            ViewBag.BookingStatuses = Enum.GetNames(typeof(BookingStatus))
                .Where(s => s != "Completed" && s != "Refunded")
                .ToList();
            
            return View("Bookings/Index", viewModel);
        }

        public async Task<IActionResult> BookingDetails(Guid id)
        {
            var query = new GetBookingByIdQuery(id);
            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
            {
                return RedirectToAction(nameof(Bookings));
            }
            return View("Bookings/Details", result.Value);
        }

        public async Task<IActionResult> UserBookings(Guid userId)
        {
            var query = new GetUserBookingsQuery(userId);
            var result = await _mediator.Send(query);
            return View("Bookings/UserBookings", result.Value);
        }
        [HttpGet]
        public async Task<IActionResult> ExportBookingsReport()
        {
            var query = new ExportBookingsQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return RedirectToAction(nameof(Bookings));
            }

            var fileName = $"bookings_report_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv";
            return File(result.Value ?? [], "text/csv", fileName);
        }
    }
}
