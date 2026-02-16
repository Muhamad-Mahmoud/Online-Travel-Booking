using Microsoft.AspNetCore.Mvc;
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

        public AdminController(OnlineTravelDbContext context, MediatR.IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var query = new OnlineTravel.Application.Features.Admin.Dashboard.GetAdminDashboardStatsQuery();
            var result = await _mediator.Send(query);
            return View(result.Value);
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

        public async Task<IActionResult> Bookings(int pageIndex = 1, int pageSize = 5, string? searchTerm = null, string? status = null)
        {
            var bookingsQuery = new GetAllBookingsQuery(pageIndex, pageSize, searchTerm, status);
            var statsQuery = new OnlineTravel.Application.Features.Bookings.GetBookingStats.GetBookingStatsQuery();

            var bookingsResult = await _mediator.Send(bookingsQuery);
            var statsResult = await _mediator.Send(statsQuery);
            
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
            
            return View(viewModel);
        }

        public async Task<IActionResult> BookingDetails(Guid id)
        {
            var query = new GetBookingByIdQuery(id);
            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
            {
                return RedirectToAction(nameof(Bookings));
            }
            return View(result.Value);
        }

        public async Task<IActionResult> UserBookings(Guid userId)
        {
            var query = new GetUserBookingsQuery(userId);
            var result = await _mediator.Send(query);
            return View(result.Value);
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
