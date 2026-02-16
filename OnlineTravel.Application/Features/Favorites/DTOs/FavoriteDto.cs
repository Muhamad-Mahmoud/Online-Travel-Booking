namespace OnlineTravel.Application.Features.Favorites.DTOs;

public record FavoriteDto(
    Guid Id,
    Guid ItemId,
    string ItemType,
    DateTime AddedAt
);
