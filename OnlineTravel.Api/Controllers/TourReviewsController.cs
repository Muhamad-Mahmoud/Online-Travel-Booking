using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Reviews.CreateReview;
using OnlineTravel.Application.Features.Reviews.GetTourReviews;

namespace OnlineTravel.Api.Controllers;

[Route("api/v1/tours/{tourId:guid}/reviews")]
public class TourReviewsController : BaseApiController
{
	/// <summary>
	/// Get all reviews for a specific tour.
	/// </summary>
	[HttpGet]
	public async Task<ActionResult> GetTourReviews(Guid tourId)
	{
		var result = await Mediator.Send(new GetTourReviewsQuery(tourId));
		return HandleResult(result);
	}

	/// <summary>
	/// Create a new review and rating for a tour.
	/// </summary>
	[Authorize]
	[HttpPost]
	public async Task<ActionResult> Create(Guid tourId, [FromBody] CreateReviewCommand request)
	{
		var command = new CreateReviewCommand(tourId, UserId, request.Rating, request.Comment);
		var result = await Mediator.Send(command);
		return HandleResult(result);
	}
}

