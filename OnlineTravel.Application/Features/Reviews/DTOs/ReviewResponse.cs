namespace OnlineTravel.Application.Features.Reviews.DTOs;

public record ReviewResponse(
	Guid Id,
	string ReviewerName,
	decimal Rating,
	string? Comment,
	DateTime CreatedAt
);
