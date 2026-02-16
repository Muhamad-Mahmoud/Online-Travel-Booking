using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Application.Features.Reviews.DTOs;

namespace OnlineTravel.Application.Features.Reviews.Queries;

public record GetTourReviewsQuery(Guid TourId) : IRequest<Result<List<ReviewResponse>>>;
