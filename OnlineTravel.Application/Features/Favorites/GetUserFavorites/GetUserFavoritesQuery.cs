using MediatR;
using OnlineTravel.Application.Features.Favorites.DTOs;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Application.Features.Favorites.GetUserFavorites;

public sealed record GetUserFavoritesQuery(
	Guid UserId
) : IRequest<Result<List<FavoriteResponse>>>;
