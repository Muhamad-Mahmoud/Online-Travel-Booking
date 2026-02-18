using MediatR;
using OnlineTravel.Application.Common;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Core;
using OnlineTravel.Domain.Entities.Hotels;
using OnlineTravel.Domain.Entities.Reviews;
using OnlineTravel.Domain.Entities.Reviews.ValueObjects;
using OnlineTravel.Domain.Enums;

namespace OnlineTravel.Application.Features.Hotels.Public.AddReview;

public class AddReviewCommandHandler : IRequestHandler<AddReviewCommand, Result<ReviewAddedResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddReviewCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ReviewAddedResponse>> Handle(AddReviewCommand request, CancellationToken cancellationToken)
    {
        var hotel = await _unitOfWork.Hotels.GetWithReviewsAsync(request.HotelId);
        if (hotel == null)
            return Result<ReviewAddedResponse>.Failure("Hotel not found");

        var categories = await _unitOfWork.Repository<Category>().GetAllAsync(cancellationToken);
        var hotelCategory = categories.FirstOrDefault(c => c.Type == CategoryType.Hotel);
        if (hotelCategory == null)
            return Result<ReviewAddedResponse>.Failure("Hotel category not found.");

        var review = new Review
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            CategoryId = hotelCategory.Id,
            ItemId = request.HotelId,
            HotelId = request.HotelId,
            Rating = new StarRating(request.Rating),
            Comment = request.Comment
        };

        hotel.AddReview(review);
        await _unitOfWork.Repository<Review>().AddAsync(review, cancellationToken);
        _unitOfWork.Hotels.Update(hotel);
        await _unitOfWork.Complete();

        return Result<ReviewAddedResponse>.Success(new ReviewAddedResponse
        {
            Id = review.Id,
            Message = "Review added."
        });
    }
}
