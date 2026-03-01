using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Reviews.CreateReview;

public record CreateReviewCommand(
	Guid TourId,
	Guid UserId,
	decimal Rating,
	string? Comment
) : IRequest<Result<Guid>>;
