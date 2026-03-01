using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Reviews;
using OnlineTravel.Domain.Entities.Reviews.ValueObjects;
using OnlineTravel.Domain.Enums;

namespace OnlineTravel.Application.Features.Reviews.CreateReview;

public class CreateReviewHandler : IRequestHandler<CreateReviewCommand, Result<Guid>>
{
	private readonly IUnitOfWork _unitOfWork;

	public CreateReviewHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<Guid>> Handle(CreateReviewCommand command, CancellationToken cancellationToken)
	{
		// Get Tour Category Id

		var tourCategory = await _unitOfWork.Repository<Domain.Entities.Core.Category>()
			.FindAsync(c => c.Type == CategoryType.Tour);

		if (tourCategory == null)
			return Result<Guid>.Failure("Tour category not found");

		// Validate Tour Exists
		var tour = await _unitOfWork.Repository<Domain.Entities.Tours.Tour>()
			.GetByIdAsync(command.TourId);

		if (tour == null)
			return Result<Guid>.Failure("Tour not found");

		// Create Review
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
