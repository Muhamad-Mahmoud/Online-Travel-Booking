using MediatR;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Favorites.AddFavorite;

public sealed record AddFavoriteCommand(
	Guid UserId,
	Guid ItemId
) : IRequest<Result<Guid>>;
