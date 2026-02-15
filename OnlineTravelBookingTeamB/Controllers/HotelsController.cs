using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Hotels.Public.AddReview;
using OnlineTravel.Application.Hotels.Public.GetHotelDetails;
using OnlineTravel.Application.Hotels.Public.GetHotelRooms;
using OnlineTravel.Application.Hotels.Public.SearchHotels;

namespace OnlineTravelBookingTeamB.Controllers
{
    [ApiController]
    [Route("api/v1/hotels")]
    public class HotelsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HotelsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> SearchHotels([FromQuery] SearchHotelsQuery query)
        {
            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
                return BadRequest(new { error = result.ErrorMessage, validationErrors = result.ValidationErrors });
            return Ok(result.Data);
        }

        [HttpGet("{id}", Name = "GetHotelDetails")]
        public async Task<IActionResult> GetHotelDetails(Guid id)
        {
            var result = await _mediator.Send(new GetHotelDetailsQuery { Id = id });
            if (!result.IsSuccess)
                return NotFound(new { error = result.ErrorMessage });
            return Ok(result.Data);
        }

        [HttpGet("{id}/rooms")]
        public async Task<IActionResult> GetHotelRooms(Guid id, [FromQuery] DateOnly? checkin, [FromQuery] DateOnly? checkout)
        {
            var result = await _mediator.Send(new GetHotelRoomsQuery
            {
                HotelId = id,
                CheckIn = checkin,
                CheckOut = checkout
            });
            if (!result.IsSuccess)
                return NotFound(new { error = result.ErrorMessage });
            return Ok(result.Data);
        }

        [HttpPost("{id}/reviews")]
        public async Task<IActionResult> AddReview(Guid id, [FromBody] AddReviewCommand command)
        {
            command.HotelId = id;
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(new { error = result.ErrorMessage, validationErrors = result.ValidationErrors });
            return CreatedAtAction(nameof(GetHotelDetails), new { id }, result.Data);
        }
    }
}
