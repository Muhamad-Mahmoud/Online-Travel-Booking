namespace OnlineTravel.Application.Features.Favorites.DTOs;

public record FavoriteResponse(
	Guid Id,
	Guid ItemId,
	string ItemType,
	DateTime AddedAt
);
