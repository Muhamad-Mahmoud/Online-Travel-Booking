using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Application.Features.Reviews.Shared;

namespace OnlineTravel.Application.Features.Reviews.Queries;

public record GetTourReviewsQuery(Guid TourId) : IRequest<OnlineTravel.Application.Common.Result<List<ReviewResponse>>>;

