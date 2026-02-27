using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Admin.Export;
using OnlineTravel.Application.Features.Bookings.GetAllBookings;
using OnlineTravel.Application.Features.Bookings.GetBookingById;
using OnlineTravel.Application.Features.Bookings.GetBookingStats;

namespace OnlineTravel.Mvc.Controllers;

public class BookingsController : BaseController
{
	public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string? searchTerm = null, string? status = null)
	{
		var bookingsResult = await Mediator.Send(new GetAllBookingsQuery(pageIndex, pageSize, searchTerm, status));
		var statsResult = await Mediator.Send(new GetBookingStatsQuery());

		var model = new OnlineTravel.Mvc.Models.BookingsIndexViewModel
		{
			Bookings = bookingsResult.Value,
			Stats = statsResult.Value
		};

		ViewBag.SearchTerm = searchTerm;
		ViewBag.Status = status;
		ViewBag.BookingStatuses = new List<string> { "Confirmed", "PendingPayment", "Cancelled", "Paid" };

		return View("~/Views/Admin/Bookings/Index.cshtml", model);
	}

	public async Task<IActionResult> Details(Guid id)
	{
		var result = await Mediator.Send(new GetBookingByIdQuery(id));
		if (result.IsSuccess)
		{
			return View("~/Views/Admin/Bookings/Details.cshtml", result.Value);
		}
		return NotFound();
	}

	public async Task<IActionResult> Export()
	{
		var result = await Mediator.Send(new ExportBookingsQuery());
		if (!result.IsSuccess)
		{
			return BadRequest();
		}

		return File(result.Value ?? Array.Empty<byte>(), "text/csv", $"bookings-{DateTime.UtcNow:yyyyMMddHHmmss}.csv");
	}
}
