using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Tours.GetTourById.Queries;
using OnlineTravel.Application.Features.Tours.GetTourById.DTOs;
using OnlineTravel.Application.Features.Tours.GetAllTours.Queries;
using OnlineTravel.Application.Features.Tours.GetAllTours.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace OnlineTravelBookingTeamB.Controllers;

public class ToursController : BaseApiController
{
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(OnlineTravel.Application.Common.PagedResult<TourResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] OnlineTravel.Application.Common.PaginationParams paginationParams,
        [FromQuery] string? search,
        [FromQuery] double? lat,
        [FromQuery] double? lon,
        [FromQuery] double? radiusKm,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] int? rating,
        [FromQuery] string? city,
        [FromQuery] string? country,
        [FromQuery] string? sortOrder)
    {
        var result = await Mediator.Send(new GetAllToursQuery(paginationParams.PageIndex, paginationParams.PageSize, search, lat, lon, radiusKm, minPrice, maxPrice, rating, city, country, sortOrder));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetTourByIdQuery(Id: id);
        var result = await Mediator.Send(query);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost("{id}/reviews")]
    [Authorize] 
    public async Task<IActionResult> CreateReview(Guid id, [FromBody] OnlineTravel.Application.Features.Reviews.DTOs.CreateReviewRequest request)
    {
        var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
             return Unauthorized("User ID not found in token.");
        }

        var command = new OnlineTravel.Application.Features.Reviews.Commands.CreateReviewCommand(
            TourId: id,
            UserId: userId, 
            Rating: request.Rating,
            Comment: request.Comment
        );

        var result = await Mediator.Send(command);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpGet("{id}/reviews")]
    [ProducesResponseType(typeof(List<OnlineTravel.Application.Features.Reviews.DTOs.ReviewResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReviews(Guid id)
    {
        var result = await Mediator.Send(new OnlineTravel.Application.Features.Reviews.Queries.GetTourReviewsQuery(id));
        
        if (!result.IsSuccess)
             return BadRequest(result.Error); // Or NotFound check inside handler

        return Ok(result.Value);
    }
}
