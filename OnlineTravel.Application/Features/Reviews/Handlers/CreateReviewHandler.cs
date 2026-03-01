using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Application.Features.Reviews.Commands;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Reviews;
using OnlineTravel.Domain.Entities.Reviews.ValueObjects;
using OnlineTravel.Domain.Enums;

namespace OnlineTravel.Application.Features.Reviews.Handlers;

public class CreateReviewHandler : IRequestHandler<CreateReviewCommand, Result<Guid>>
{
	private readonly IUnitOfWork _unitOfWork;

	public CreateReviewHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<Guid>> Handle(CreateReviewCommand command, CancellationToken cancellationToken)
	{
		// 1. Get Tour Category Id
		// Assuming we need a way to get Category by type from Repository.
		// Repository pattern typically uses Specifications here.
		// Since we don't have a spec for Category yet, we might need one or generic FindAsync.

		var tourCategory = await _unitOfWork.Repository<OnlineTravel.Domain.Entities.Core.Category>()
			.FindAsync(c => c.Type == CategoryType.Tour);

		if (tourCategory == null)
			return Result<Guid>.Failure("Tour category not found");

		// 2. Validate Tour Exists
		var tour = await _unitOfWork.Repository<OnlineTravel.Domain.Entities.Tours.Tour>()
			.GetByIdAsync(command.TourId);

		if (tour == null)
			return Result<Guid>.Failure("Tour not found");

		// 3. Create Review
		var review = new Review
		{
			UserId = command.UserId,
			CategoryId = tourCategory.Id,
			ItemId = command.TourId,
			Rating = new StarRating(command.Rating),
			Comment = command.Comment,
			CreatedAt = DateTime.UtcNow
		};

		await _unitOfWork.Repository<Review>().AddAsync(review);
		await _unitOfWork.SaveChangesAsync();

		return Result<Guid>.Success(review.Id);
	}
}
