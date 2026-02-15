using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Hotels.Admin.AddRoom;
using OnlineTravel.Application.Hotels.Admin.ConfigureSeasonalPricing;
using OnlineTravel.Application.Hotels.Admin.CreateHotelCommand;
using OnlineTravel.Application.Hotels.Admin.EditRoom;
using OnlineTravel.Application.Hotels.Admin.ManageAvailability;
using OnlineTravel.Application.Hotels.Admin.UpdateHotel;

namespace OnlineTravelBookingTeamB.Controllers
{
    [ApiController]
    [Route("api/v1/admin")]
    public class AdminHotelsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminHotelsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("hotels")]
        public async Task<IActionResult> CreateHotel([FromBody] CreateHotelCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.ErrorMessage, validationErrors = result.ValidationErrors });

            return CreatedAtRoute("GetHotelDetails", new { id = result.Data.Id }, result.Data);
        }

        [HttpPut("hotels/{id}")]
        public async Task<IActionResult> UpdateHotel(Guid id, [FromBody] UpdateHotelCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.ErrorMessage, validationErrors = result.ValidationErrors });

            return Ok(result.Data);
        }

        [HttpPost("hotels/{hotelId}/rooms")]
        public async Task<IActionResult> AddRoom(Guid hotelId, [FromBody] AddRoomCommand command)
        {
            command.HotelId = hotelId;
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.ErrorMessage, validationErrors = result.ValidationErrors });

            return CreatedAtAction(nameof(HotelsController.GetHotelRooms), "Hotels", new { id = hotelId }, result.Data);
        }

        [HttpPut("rooms/{id}")]
        public async Task<IActionResult> EditRoom(Guid id, [FromBody] EditRoomCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.ErrorMessage, validationErrors = result.ValidationErrors });

            return Ok(result.Data);
        }

        [HttpPut("rooms/{id}/availability")]
        public async Task<IActionResult> ManageAvailability(Guid id, [FromBody] ManageAvailabilityCommand command)
        {
            command.RoomId = id;
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.ErrorMessage, validationErrors = result.ValidationErrors });

            return Ok(result.Data);
        }

        [HttpPost("rooms/{id}/seasonal-pricing")]
        public async Task<IActionResult> ConfigureSeasonalPricing(Guid id, [FromBody] ConfigureSeasonalPricingCommand command)
        {
            command.RoomId = id;
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.ErrorMessage, validationErrors = result.ValidationErrors });

            return Ok(result.Data);
        }
    }
}
