using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OnlineTravelBookingTeamB.Extensions;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Application.Features.Bookings.CreateBooking;
using OnlineTravel.Application.Features.Bookings.GetBookingById;
using OnlineTravel.Application.Features.Bookings.CancelBooking;
using OnlineTravel.Application.Features.Bookings.GetUserBookings;
using OnlineTravel.Application.Features.Bookings.GetAllBookings;

namespace OnlineTravelBookingTeamB.Controllers
{
    /// <summary>
    /// Controller for managing flight, hotel, tour, and car bookings.
    /// </summary>
    [Authorize]
    public class BookingsController : BaseApiController
    {
        /// <summary>
        /// Creates a new booking for the authenticated user.
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Create([FromBody] CreateBookingCommand command)
        {
            var commandWithUser = command with { UserId = UserId };
            var result = await Mediator.Send(commandWithUser);
            return result.ToResponse(201);
        }

        /// <summary>
        /// Retrieves all bookings for the currently authenticated user.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> GetMyBookings([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetUserBookingsQuery(UserId, pageIndex, pageSize);
            var result = await Mediator.Send(query);
            return result.ToResponse();
        }

        /// <summary>
        /// Cancels an existing booking for the authenticated user.
        /// </summary>
        [HttpPost("cancel")]
        public async Task<ActionResult> Cancel([FromBody] CancelBookingCommand command)
        {
            var commandWithUser = command with { UserId = UserId };
            var result = await Mediator.Send(commandWithUser);
            return result.ToResponse();
        }

        /// <summary>
        /// Retrieves a specific booking by its unique identifier.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            var query = new GetBookingByIdQuery(id);
            var result = await Mediator.Send(query);
            return result.ToResponse();
        }

        /// <summary>
        /// Retrieves all bookings for a specific user.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("user/{userId}")]
        public async Task<ActionResult> GetUserBookings(Guid userId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetUserBookingsQuery(userId, pageIndex, pageSize);
            var result = await Mediator.Send(query);
            return result.ToResponse();
        }

        /// <summary>
        /// Retrieves a paginated list of all bookings in the system.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("admin/all")]
        public async Task<ActionResult> GetAllBookings([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetAllBookingsQuery(pageIndex, pageSize);
            var result = await Mediator.Send(query);
            return result.ToResponse();
        }
    }
}
