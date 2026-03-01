using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Application.Features.Reviews.Queries;
using OnlineTravel.Application.Features.Reviews.Shared;
using OnlineTravel.Application.Features.Reviews.Specifications;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Reviews;
using OnlineTravel.Domain.Enums;

namespace OnlineTravel.Application.Features.Reviews.Handlers;

public class GetTourReviewsHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetTourReviewsQuery, Result<List<ReviewResponse>>>
{


	public async Task<Result<List<ReviewResponse>>> Handle(GetTourReviewsQuery request, CancellationToken cancellationToken)

	{
		// 1. Get Tour Category Id
		var tourCategory = await unitOfWork.Repository<OnlineTravel.Domain.Entities.Core.Category>()
			.FindAsync(c => c.Type == CategoryType.Tour, cancellationToken);


		if (tourCategory == null)
			return Result<List<ReviewResponse>>.Failure("Tour category not found");


		// 2. Query Reviews
		var spec = new TourReviewsSpecification(request.TourId, tourCategory.Id);
		var reviews = await unitOfWork.Repository<Review>().GetAllWithSpecAsync(spec, cancellationToken);


		var response = reviews.Select(r => new ReviewResponse
		{
			Id = r.Id,
			UserName = r.User?.Name ?? "Anonymous",
			Rating = (int)r.Rating.Value,
			Comment = r.Comment ?? string.Empty,
			CreatedAt = r.CreatedAt
		}).ToList();

		return Result<List<ReviewResponse>>.Success(response);

	}
}

