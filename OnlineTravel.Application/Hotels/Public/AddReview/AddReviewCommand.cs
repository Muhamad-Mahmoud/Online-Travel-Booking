using MediatR;
using OnlineTravel.Application.Hotels.Common;

namespace OnlineTravel.Application.Hotels.Public.AddReview;

public class AddReviewCommand : IRequest<Result<ReviewAddedResponse>>
{
    public Guid HotelId { get; set; }
    public Guid UserId { get; set; }
    /// <summary>Rating from 1 to 5.</summary>
    public int Rating { get; set; }
    public string? Comment { get; set; }
}

public class ReviewAddedResponse
{
    public Guid Id { get; set; }
    public string Message { get; set; } = "Review added.";
}
