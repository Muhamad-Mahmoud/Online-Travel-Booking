using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Bookings.GetAllBookings;
using OnlineTravel.Application.Features.Bookings.GetBookingById;
using OnlineTravel.Application.Features.Bookings.GetBookingStats;
using OnlineTravel.Application.Features.Bookings.GetUserBookings;
using OnlineTravel.Application.Features.Admin.Export;
using OnlineTravel.Application.Features.Bookings.Shared.DTOs;
using OnlineTravel.Domain.Enums;
using MediatR;

namespace OnlineTravelBookingTeamB.Controllers.Admin
{
    [Route("Admin/Bookings")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class BookingsController : Controller
    {
        private readonly IMediator _mediator;

        public BookingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 5, string? searchTerm = null, string? status = null)
        {
            var bookingsQuery = new GetAllBookingsQuery(pageIndex, pageSize, searchTerm, status);
            var statsQuery = new GetBookingStatsQuery();

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
            
            return View("~/Views/Admin/Bookings/Index.cshtml", viewModel);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var query = new GetBookingByIdQuery(id);
            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            return View("~/Views/Admin/Bookings/Details.cshtml", result.Value);
        }

        [HttpGet("User/{userId}")]
        public async Task<IActionResult> UserBookings(Guid userId)
        {
            var query = new GetUserBookingsQuery(userId);
            var result = await _mediator.Send(query);
            return View("~/Views/Admin/Bookings/UserBookings.cshtml", result.Value);
        }

        [HttpGet("Export")]
        public async Task<IActionResult> Export()
        {
            var query = new ExportBookingsQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }

            var fileName = $"bookings_report_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv";
            return File(result.Value ?? [], "text/csv", fileName);
        }
    }
}
