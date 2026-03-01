using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Hotels.Admin.AddRoom;
using OnlineTravel.Application.Features.Hotels.Admin.ConfigureSeasonalPricing;
using OnlineTravel.Application.Features.Hotels.Admin.CreateHotel;
using OnlineTravel.Application.Features.Hotels.Admin.EditRoom;
using OnlineTravel.Application.Features.Hotels.Admin.ManageAvailability;
using OnlineTravel.Application.Features.Hotels.Admin.GetHotels;
using OnlineTravel.Application.Features.Hotels.Admin.DeleteRoom;
using OnlineTravel.Application.Features.Hotels.Admin.UpdateHotel;
using OnlineTravel.Application.Features.Hotels.Public.AddReview;
using OnlineTravel.Application.Features.Hotels.Public.GetHotelDetails;
using OnlineTravel.Application.Features.Hotels.Public.GetHotelRooms;
using OnlineTravel.Application.Features.Hotels.Public.SearchHotels;

namespace OnlineTravel.Api.Controllers;

[Route("api/v1/hotels")]
public class HotelsController : BaseApiController
{
	/// <summary>
	/// Search for hotels based on location, dates, and occupancy.
	/// </summary>
	[HttpGet]
	public async Task<IActionResult> SearchHotels([FromQuery] SearchHotelsQuery query)
	{
		var result = await Mediator.Send(query);
		return HandleResult(result);
	}

	/// <summary>
	/// Get detailed information for a specific hotel.
	/// </summary>
	[HttpGet("{id}", Name = "GetHotelDetails")]
	public async Task<IActionResult> GetHotelDetails(Guid id)
	{
		var result = await Mediator.Send(new GetHotelDetailsQuery { Id = id });
		return HandleResult(result);
	}

	/// <summary>
	/// Get available rooms for a specific hotel and date range.
	/// </summary>
	[HttpGet("{id}/rooms")]
	public async Task<IActionResult> GetHotelRooms(Guid id, [FromQuery] DateOnly? checkin, [FromQuery] DateOnly? checkout)
	{
		var result = await Mediator.Send(new GetHotelRoomsQuery
		{
			HotelId = id,
			CheckIn = checkin,
			CheckOut = checkout
		});
		return HandleResult(result);
	}

	/// <summary>
	/// Add a review and rating for a hotel.
	/// </summary>
	[HttpPost("{id}/reviews")]
	public async Task<IActionResult> AddReview(Guid id, [FromBody] AddReviewCommand command)
	{
		command.HotelId = id;
		var result = await Mediator.Send(command);
		return HandleResult(result);
	}

	/// <summary>
	/// Create a new hotel entry (Admin only).
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreateHotelCommand command)
	{
		var result = await Mediator.Send(command);
		return HandleResult(result);
	}

	/// <summary>
	/// Update information for an existing hotel (Admin only).
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpPut("{id:guid}")]
	public async Task<IActionResult> Update(Guid id, [FromBody] UpdateHotelCommand command)
	{
		command.Id = id;
		var result = await Mediator.Send(command);
		return HandleResult(result);
	}

	/// <summary>
	/// Get a paginated list of all hotels for administrative management (Admin only).
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpGet("admin")]
	public async Task<IActionResult> GetAllAdmin([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
	{
		var result = await Mediator.Send(new GetHotelsQuery(pageIndex, pageSize, search));
		return HandleResult(result);
	}

	/// <summary>
	/// Add a new room type to a hotel (Admin only).
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpPost("{id:guid}/rooms")]
	public async Task<IActionResult> AddRoom(Guid id, [FromBody] AddRoomCommand command)
	{
		command.HotelId = id;
		var result = await Mediator.Send(command);
		return HandleResult(result);
	}

	/// <summary>
	/// Update room details for a specific room type (Admin only).
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpPut("rooms/{roomId:guid}")]
	public async Task<IActionResult> EditRoom(Guid roomId, [FromBody] EditRoomCommand command)
	{
		command.Id = roomId;
		var result = await Mediator.Send(command);
		return HandleResult(result);
	}

	/// <summary>
	/// Manage room availability for specific dates (Admin only).
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpPost("rooms/{roomId:guid}/availability")]
	public async Task<IActionResult> ManageAvailability(Guid roomId, [FromBody] ManageAvailabilityCommand command)
	{
		command.RoomId = roomId;
		var result = await Mediator.Send(command);
		return HandleResult(result);
	}

	/// <summary>
	/// Configure seasonal pricing adjustments for a room type (Admin only).
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpPost("rooms/{roomId:guid}/seasonal-pricing")]
	public async Task<IActionResult> ConfigureSeasonalPricing(Guid roomId, [FromBody] ConfigureSeasonalPricingCommand command)
	{
		command.RoomId = roomId;
		var result = await Mediator.Send(command);
		return HandleResult(result);
	}

	/// <summary>
	/// Delete a room type from a hotel (Admin only).
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpDelete("rooms/{roomId:guid}")]
	public async Task<IActionResult> DeleteRoom(Guid roomId)
	{
		var result = await Mediator.Send(new DeleteRoomCommand { Id = roomId });
		return HandleResult(result);
	}
}

