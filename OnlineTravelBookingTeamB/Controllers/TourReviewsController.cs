using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Reviews.Commands;
using OnlineTravel.Application.Features.Reviews.DTOs;
using OnlineTravel.Application.Features.Reviews.Queries;

namespace OnlineTravelBookingTeamB.Controllers;

[Route("api/v1/tours/{tourId:guid}/reviews")]
public class TourReviewsController : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult> GetTourReviews(Guid tourId)
    {
        var result = await Mediator.Send(new GetTourReviewsQuery(tourId));
        return HandleResult(result);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> Create(Guid tourId, [FromBody] CreateReviewRequest request)
    {
        var command = new CreateReviewCommand(tourId, UserId, request.Rating, request.Comment);
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }
}
