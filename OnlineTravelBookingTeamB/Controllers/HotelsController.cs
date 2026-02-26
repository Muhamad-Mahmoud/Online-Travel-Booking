using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OnlineTravel.Application.Features.Hotels.Admin.AddRoom;
using OnlineTravel.Application.Features.Hotels.Admin.ConfigureSeasonalPricing;
using OnlineTravel.Application.Features.Hotels.Admin.CreateHotelCommand;
using OnlineTravel.Application.Features.Hotels.Admin.EditRoom;
using OnlineTravel.Application.Features.Hotels.Admin.ManageAvailability;
using OnlineTravel.Application.Features.Hotels.Admin.UpdateHotel;
using OnlineTravel.Application.Features.Hotels.Public.AddReview;
using OnlineTravel.Application.Features.Hotels.Public.GetHotelDetails;
using OnlineTravel.Application.Features.Hotels.Public.GetHotelRooms;
using OnlineTravel.Application.Features.Hotels.Public.SearchHotels;

namespace OnlineTravelBookingTeamB.Controllers;

[Route("api/v1/hotels")]
public class HotelsController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> SearchHotels([FromQuery] SearchHotelsQuery query)
    {
        var result = await Mediator.Send(query);
        return HandleResult(result);
    }

    [HttpGet("{id}", Name = "GetHotelDetails")]
    public async Task<IActionResult> GetHotelDetails(Guid id)
    {
        var result = await Mediator.Send(new GetHotelDetailsQuery { Id = id });
        return HandleResult(result);
    }

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

    [HttpPost("{id}/reviews")]
    public async Task<IActionResult> AddReview(Guid id, [FromBody] AddReviewCommand command)
    {
        command.HotelId = id;
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateHotelCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateHotelCommand command)
    {
        command.Id = id;
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{id:guid}/rooms")]
    public async Task<IActionResult> AddRoom(Guid id, [FromBody] AddRoomCommand command)
    {
        command.HotelId = id;
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("rooms/{roomId:guid}")]
    public async Task<IActionResult> EditRoom(Guid roomId, [FromBody] EditRoomCommand command)
    {
        command.Id = roomId;
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("rooms/{roomId:guid}/availability")]
    public async Task<IActionResult> ManageAvailability(Guid roomId, [FromBody] ManageAvailabilityCommand command)
    {
        command.RoomId = roomId;
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("rooms/{roomId:guid}/seasonal-pricing")]
    public async Task<IActionResult> ConfigureSeasonalPricing(Guid roomId, [FromBody] ConfigureSeasonalPricingCommand command)
    {
        command.RoomId = roomId;
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }
}
