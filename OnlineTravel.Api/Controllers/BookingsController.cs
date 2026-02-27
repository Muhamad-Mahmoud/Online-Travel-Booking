using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Admin.Export;
using OnlineTravel.Application.Features.Bookings.CancelBooking;
using OnlineTravel.Application.Features.Bookings.CreateBooking;
using OnlineTravel.Application.Features.Bookings.GetAllBookings;
using OnlineTravel.Application.Features.Bookings.GetBookingById;
using OnlineTravel.Application.Features.Bookings.GetBookingStats;
using OnlineTravel.Application.Features.Bookings.GetUserBookings;

namespace OnlineTravel.Api.Controllers;

[Authorize]
[Route("api/v1/bookings")]
public class BookingsController : BaseApiController
{
	[HttpPost]
	public async Task<ActionResult> Create([FromBody] CreateBookingCommand command)
	{
		var commandWithUser = command with { UserId = UserId };
		var result = await Mediator.Send(commandWithUser);
		return HandleResult(result);
	}

	[HttpGet]
	public async Task<ActionResult> GetMyBookings([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
	{
		var query = new GetUserBookingsQuery(UserId, pageIndex, pageSize);
		var result = await Mediator.Send(query);
		return HandleResult(result);
	}

	[HttpPost("cancel")]
	public async Task<ActionResult> Cancel([FromBody] CancelBookingCommand command)
	{
		var commandWithUser = command with { UserId = UserId };
		var result = await Mediator.Send(commandWithUser);
		return HandleResult(result);
	}

	[Authorize(Roles = "Admin")]
	[HttpGet("admin")]
	public async Task<ActionResult> GetAll([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null, [FromQuery] string? status = null)
	{
		var result = await Mediator.Send(new GetAllBookingsQuery(pageIndex, pageSize, searchTerm, status));
		return HandleResult(result);
	}

	[Authorize(Roles = "Admin")]
	[HttpGet("admin/stats")]
	public async Task<ActionResult> GetStats()
	{
		var result = await Mediator.Send(new GetBookingStatsQuery());
		return HandleResult(result);
	}

	[Authorize(Roles = "Admin")]
	[HttpGet("admin/{id:guid}")]
	public async Task<ActionResult> GetById(Guid id)
	{
		var result = await Mediator.Send(new GetBookingByIdQuery(id));
		return HandleResult(result);
	}

	[Authorize(Roles = "Admin")]
	[HttpGet("admin/users/{userId:guid}")]
	public async Task<ActionResult> GetByUserId(Guid userId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
	{
		var result = await Mediator.Send(new GetUserBookingsQuery(userId, pageIndex, pageSize));
		return HandleResult(result);
	}

	[Authorize(Roles = "Admin")]
	[HttpGet("admin/export")]
	public async Task<ActionResult> Export()
	{
		var result = await Mediator.Send(new ExportBookingsQuery());
		if (!result.IsSuccess)
		{
			return HandleResult(result);
		}

		if (result.Value is null || result.Value.Length == 0)
		{
			return NotFound("No export data available.");
		}

		return File(result.Value, "text/csv", $"bookings-{DateTime.UtcNow:yyyyMMddHHmmss}.csv");
	}
}

