using System.ComponentModel.DataAnnotations;

namespace OnlineTravel.Application.Features.Reviews.DTOs;

public record CreateReviewRequest
{
	[Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
	public decimal Rating { get; init; }

	[MaxLength(1000, ErrorMessage = "Comment cannot exceed 1000 characters")]
	public string? Comment { get; init; }
}
