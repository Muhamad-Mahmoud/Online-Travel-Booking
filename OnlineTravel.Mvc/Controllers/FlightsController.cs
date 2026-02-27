using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Flight.Flights.GetFlights;

namespace OnlineTravel.Mvc.Controllers;

public class FlightsController : BaseController
{
	public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string? search = null, string? status = null)
	{
		var result = await Mediator.Send(new GetFlightsQuery(pageIndex, pageSize, search, status));
		ViewBag.SearchTerm = search;
		ViewBag.Status = status;
		ViewBag.FlightStatuses = new List<string> { "Scheduled", "Cancelled", "Delayed", "InAir", "Landed" };
		return View("~/Views/Admin/Flights/Flights/Index.cshtml", result.Value);
	}
}
