using MediatR;
using OnlineTravel.Application.Common;

namespace OnlineTravel.Application.Features.Reviews.Commands;

public record CreateReviewCommand(
    Guid TourId,
    Guid UserId,
    decimal Rating,
    string? Comment
) : IRequest<Result<Guid>>;
