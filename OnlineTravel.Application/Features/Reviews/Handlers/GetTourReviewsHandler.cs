using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Application.Features.Reviews.DTOs;
using OnlineTravel.Application.Features.Reviews.Queries;
using OnlineTravel.Application.Features.Reviews.Specifications;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Reviews;
using OnlineTravel.Domain.Enums;

namespace OnlineTravel.Application.Features.Reviews.Handlers;

public class GetTourReviewsHandler : IRequestHandler<GetTourReviewsQuery, Result<List<ReviewResponse>>>
{
	private readonly IUnitOfWork _unitOfWork;

	public GetTourReviewsHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<List<ReviewResponse>>> Handle(GetTourReviewsQuery request, CancellationToken cancellationToken)
	{
		// 1. Get Tour Category Id
		var tourCategory = await _unitOfWork.Repository<OnlineTravel.Domain.Entities.Core.Category>()
			.FindAsync(c => c.Type == CategoryType.Tour);

		if (tourCategory == null)
			return Result<List<ReviewResponse>>.Failure("Tour category not found");

		// 2. Query Reviews
		var spec = new TourReviewsSpecification(request.TourId, tourCategory.Id);
		var reviews = await _unitOfWork.Repository<Review>().GetAllWithSpecAsync(spec);

		var response = reviews.Select(r => new ReviewResponse(
				r.Id,
				r.User?.Name ?? "Anonymous",
				r.Rating.Value,
				r.Comment,
				r.CreatedAt
			)).ToList();

		return Result<List<ReviewResponse>>.Success(response);
	}
}
