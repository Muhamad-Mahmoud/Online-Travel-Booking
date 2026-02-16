using OnlineTravel.Domain.Enums;

namespace OnlineTravel.Application.Features.Favorites.DTOs;

public record AddFavoriteRequest(
    Guid ItemId
);
