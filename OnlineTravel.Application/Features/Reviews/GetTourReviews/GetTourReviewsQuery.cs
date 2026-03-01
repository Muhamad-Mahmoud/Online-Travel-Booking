using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Reviews.GetTourReviews;

public record GetTourReviewsQuery(Guid TourId) : IRequest<Result<List<ReviewResponse>>>;

